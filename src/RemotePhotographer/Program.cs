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
});

builder.Services.AddControllers();

builder.Services.AddGraphQLServer()
    .AddQueryType<PhotographerQuery>()
    .AddMutationType<PhotographerMutation>()
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);


builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CommandConsumer<CaptureImage>>();
    x.AddConsumer<CommandConsumer<ConnectCamera>>();
    x.AddConsumer<CommandConsumer<DisconnectCamera>>();
    x.AddConsumer<CommandConsumer<SetAperture>>();
    x.AddConsumer<CommandConsumer<SetISO>>();
    x.AddConsumer<CommandConsumer<SetShutterSpeed>>();

    x.AddConsumer<QueryConsumer<GetAperture>>();
    x.AddConsumer<QueryConsumer<GetCameras>>();
    x.AddConsumer<QueryConsumer<GetFiles>>();
    x.AddConsumer<QueryConsumer<GetFolders>>();
    x.AddConsumer<QueryConsumer<GetImage>>();
    x.AddConsumer<QueryConsumer<GetISO>>();
    x.AddConsumer<QueryConsumer<GetShutterSpeed>>();

    x.UsingRabbitMq((context, configuration) =>
    {
        configuration.UseTimeout(c => c.Timeout = TimeSpan.FromSeconds(120));

        configuration.Host(builder.Configuration.GetValue<string>("message.broker-service:parameters:host"), "/", h =>
        {
            h.Username(builder.Configuration.GetValue<string>("message.broker-service:parameters:username"));
            h.Password(builder.Configuration.GetValue<string>("message.broker-service:parameters:password"));
        });

        configuration.ReceiveEndpoint(builder.Configuration.GetValue<string>("message.broker-service:parameters:receive.endpoint"), e =>
        {
            e.ConfigureConsumers(context);
        });
    });
}).AddMassTransitHostedService(true);

builder.Services.AddGenericRequestClient();

var app = builder.Build();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL();
    endpoints.MapControllers();
});

app.MapGet("/", () =>  "Hello");

app.Run();

public partial class Program { }