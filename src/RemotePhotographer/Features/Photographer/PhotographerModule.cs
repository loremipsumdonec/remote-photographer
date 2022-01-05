using Autofac;

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
    }
}

