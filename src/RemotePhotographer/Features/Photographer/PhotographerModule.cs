using RemotePhotographer.Features.Templates.Services;
using Autofac;
using Boilerplate.Features.Core.Config;
using RemotePhotographer.Features.Photographer.Services;

namespace RemotePhotographer.Features.Photographer;

public class PhotographerModule
        : Autofac.Module
{
    public PhotographerModule(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterFromAs<ICameraService>(
                "photographer.camera.service",
                Configuration
        ).InstancePerLifetimeScope();
    }

    private void ValidateConfiguration() 
    {
    }
}

