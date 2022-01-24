using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using Boilerplate.Features.Reactive.Services;
using RemotePhotographer.Features.Gphoto2.Services;
using RemotePhotographer.Features.Gphoto2.Services.Interop;
using RemotePhotographer.Features.Photographer.Commands;
using RemotePhotographer.Features.Photographer.Events;

namespace RemotePhotographer.Features.Gphoto2.Commands;

[Handle(typeof(DisconnectCamera))]
public class DisconnectCameraHandler
    : CommandHandler<DisconnectCamera>
{
    private readonly ICameraContextManager _manager;
    private IMethodValidator _validator;
    private IEventDispatcher _dispatcher;

    public DisconnectCameraHandler(
        ICameraContextManager manager, 
        IMethodValidator validator, 
        IEventDispatcher dispatcher)
    {
        _manager = manager;
        _validator = validator;
        _dispatcher = dispatcher;
    }

    public override Task<bool> ExecuteAsync(DisconnectCamera query)
    {
        lock(_manager.Door) 
        {
            _manager.EnsureCameraContext();

            var cameraContext = _manager.CameraContext;
            _manager.CameraContext = null;

            _validator.Validate(
                CameraService.gp_camera_exit(cameraContext.Camera, cameraContext.Context),
                nameof(CameraService.gp_camera_exit)
            );

            _validator.Validate(
                CameraService.gp_camera_free(cameraContext.Camera),
                nameof(CameraService.gp_camera_free)
            );

            ContextService.gp_context_unref(cameraContext.Context);
        }

        _dispatcher.Dispatch(new CameraDisconnected());

        return Task.FromResult(true);
    }
}