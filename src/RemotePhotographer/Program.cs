using Autofac;
using Autofac.Extensions.DependencyInjection;
using Boilerplate.Features.Core;
using Boilerplate.Features.Mapper;
using Boilerplate.Features.MassTransit.Services;
using Boilerplate.Features.Reactive.Reactive;
using MassTransit;
using System.Reflection;
using RemotePhotographer.Features.Gphoto2;
using RemotePhotographer.Features.Photographer.Schema;
using RemotePhotographer.Features.Photographer.Queries;
using RemotePhotographer.Features.Photographer.Commands;
using Boilerplate.Features.MassTransit;
using RemotePhotographer.Features.Gphoto2.Services;
using MassTransit.MongoDbIntegration.MessageData;
using RemotePhotographer.Features.Auto.Services;
using RemotePhotographer.Features.Auto.Schema;
using RemotePhotographer.Features.Auto;
using RemotePhotographer.Features.Auto.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer((ContainerBuilder containerBuilder) =>
{
    List<Assembly> assemblies = new List<Assembly>();
    assemblies.Add(Assembly.Load(new AssemblyName("RemotePhotographer")));
    assemblies.Add(Assembly.Load(new AssemblyName("Boilerplate")));

    containerBuilder.RegisterModule(new CoreModule(builder.Configuration, assemblies));
    containerBuilder.RegisterModule(new MapperModule(builder.Configuration, assemblies));
    containerBuilder.RegisterModule(new ReactiveModule(builder.Configuration, assemblies));
    containerBuilder.RegisterModule(new MassTransitModule(builder.Configuration, assemblies));
    containerBuilder.RegisterModule(new Gphoto2Module(builder.Configuration));
    containerBuilder.RegisterModule(new AutoModule(builder.Configuration));        
});

builder.Services.AddControllers();
builder.Services.AddInMemorySubscriptions();

builder.Services.AddHostedService(p => p.GetRequiredService<CapturePreviewBackgroundService>());
builder.Services.AddHostedService(p => p.GetRequiredService<SessionBackgroundService>());

builder.Services.AddGraphQLServer()
    .AddQueryType<PhotographerQuery>()
    .AddMutationType<PhotographerMutation>()
    .AddSubscriptionType<PhotographerSubscription>()
    .AddType<SessionInputSchemaType>()
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CommandConsumer<CaptureImage>>();
    x.AddConsumer<CommandConsumer<ConnectCamera>>();
    x.AddConsumer<CommandConsumer<DisconnectCamera>>();
    x.AddConsumer<CommandConsumer<SetAperture>>();
    x.AddConsumer<CommandConsumer<SetISO>>();
    x.AddConsumer<CommandConsumer<SetShutterSpeed>>();

    x.AddConsumer<CommandConsumer<StartSession>>();
    x.AddConsumer<CommandConsumer<StopSession>>();

    x.AddConsumer<QueryConsumer<GetAperture>>();
    x.AddConsumer<QueryConsumer<GetCameras>>();
    x.AddConsumer<QueryConsumer<GetFiles>>();
    x.AddConsumer<QueryConsumer<GetFolders>>();
    x.AddConsumer<QueryConsumer<GetImage>>();
    x.AddConsumer<QueryConsumer<GetISO>>();
    x.AddConsumer<QueryConsumer<GetShutterSpeed>>();

    x.UsingRabbitMq((context, configuration) =>
    {
        configuration.UseJsonSerializer();

        configuration.UseMessageData(
            new MongoDbMessageDataRepository(
                builder.Configuration.GetValue<string>("message.broker-service:parameters:data.repository.connectionString"), 
                builder.Configuration.GetValue<string>("message.broker-service:parameters:data.repository.database")
            )
        );
        
        configuration.UseTimeout(c => c.Timeout = TimeSpan.FromSeconds(120));
        configuration.Host(
            builder.Configuration.GetValue<string>("message.broker-service:parameters:host"),
            builder.Configuration.GetValue<ushort>("message.broker-service:parameters:port"),
             "/", h =>
        {
            h.Username(builder.Configuration.GetValue<string>("message.broker-service:parameters:username"));
            h.Password(builder.Configuration.GetValue<string>("message.broker-service:parameters:password"));
        });

        configuration.ReceiveEndpoint(builder.Configuration.GetValue<string>("message.broker-service:parameters:receive.endpoint"), e =>
        {
            e.ConfigureConsumers(context);
        });
    });
}).AddMassTransitHostedService();

builder.Services.AddGenericRequestClient();

var app = builder.Build();
app.UseRouting();
app.UseWebSockets();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL();
    endpoints.MapControllers();
});

app.MapGet("/", () =>  "remote photographer service");

app.Run();

public partial class Program { }