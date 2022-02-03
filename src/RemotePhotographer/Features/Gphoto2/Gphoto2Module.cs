using Autofac;
using Boilerplate.Features.Core.Config;
using RemotePhotographer.Features.Auto.Services;
using RemotePhotographer.Features.Gphoto2.Services;

namespace RemotePhotographer.Features.Gphoto2;

public class Gphoto2Module
    : Autofac.Module
{
    public Gphoto2Module(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterFromAs<ICameraContextManager>(
            "gphoto2.camera.context.manager",
            Configuration
        ).SingleInstance();

        builder.RegisterFromAs<IMethodValidator>(
            "gphoto2.method.validator",
            Configuration
        ).SingleInstance();

        builder.RegisterType<CapturePreviewBackgroundService>()
            .SingleInstance();
    }
}

