using Autofac;
using RemotePhotographer.Features.Auto.Services;

namespace RemotePhotographer.Features.Auto;

public class AutoModule
    : Autofac.Module
{
    public AutoModule(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<SessionBackgroundService>()
            .AsImplementedInterfaces()
            .SingleInstance();
    }
}

