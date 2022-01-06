using Autofac;
using Boilerplate.Features.Core.Queries;
using RemotePhotographer.Features.Photographer.Models;
using RemotePhotographer.Features.Photographer.Queries;

namespace RemotePhotographer.Features.Photographer.Schema;

public class CameraType 
    : ObjectType<Camera>
{
    protected override void Configure(IObjectTypeDescriptor<Camera> descriptor)
    {
        descriptor.Field("iso").Resolve(async (context, ct) =>
        {
            var area = context.Parent<Camera>();

            ILifetimeScope lifeTimeScope = context.Service<ILifetimeScope>();
            var dispatcher = lifeTimeScope.Resolve<IQueryDispatcher>();

            return await dispatcher.DispatchAsync<ISO>(new GetISO());
        });

        descriptor.Field("shutterSpeed").Resolve(async (context, ct) =>
        {
            var area = context.Parent<Camera>();

            ILifetimeScope lifeTimeScope = context.Service<ILifetimeScope>();
            var dispatcher = lifeTimeScope.Resolve<IQueryDispatcher>();

            return await dispatcher.DispatchAsync<ShutterSpeed>(new GetShutterSpeed());
        });
    }
}