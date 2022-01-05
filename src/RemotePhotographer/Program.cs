using Autofac;
using Autofac.Extensions.DependencyInjection;
using Boilerplate.Features.Core;
using Boilerplate.Features.Mapper;
using Boilerplate.Features.MassTransit.Services;
using Boilerplate.Features.Reactive.Reactive;
using MassTransit;
using System.Reflection;
using RemotePhotographer.Features.Photographer;
using RemotePhotographer.Features.Gphoto2;

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
    containerBuilder.RegisterModule(new Gphoto2Module(builder.Configuration));    
});

/*
builder.Services.AddGraphQLServer()
    .AddQueryType<TemplatesQuery>()
    .AddMutationType<TemplatesMutation>();
*/


/*
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<EventConsumer<TemplateCreated>>();
    x.AddConsumer<EventConsumer<TemplateDeleted>>();
    x.AddConsumer<EventConsumer<TemplateUpdated>>();

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
});
*/

var app = builder.Build();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    //endpoints.MapGraphQL();
});

app.MapGet("/", () =>  "Hello");

app.Run();

public partial class Program { }