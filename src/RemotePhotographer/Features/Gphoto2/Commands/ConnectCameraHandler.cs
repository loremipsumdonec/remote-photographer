using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using Boilerplate.Features.Reactive.Services;
using RemotePhotographer.Features.Gphoto2.Models;
using RemotePhotographer.Features.Gphoto2.Services;
using RemotePhotographer.Features.Gphoto2.Services.Interop;
using RemotePhotographer.Features.Photographer.Commands;
using RemotePhotographer.Features.Photographer.Events;

namespace RemotePhotographer.Features.Gphoto2.Commands;

[Handle(typeof(ConnectCamera))]
public class ConnectCameraHandler
    : CommandHandler<ConnectCamera>
{
    private readonly ICameraContextManager _manager;
    private IMethodValidator _validator;
    private IEventDispatcher _dispatcher;

    public ConnectCameraHandler(
        ICameraContextManager manager,
        IMethodValidator validator, 
        IEventDispatcher dispatcher)
    {
        _manager = manager;
        _validator = validator;
        _dispatcher = dispatcher;
    }

    public override Task<bool> ExecuteAsync(ConnectCamera query)
    {
        var context = ContextService.gp_context_new();

        _validator.Validate(
            CameraService.gp_camera_new(out IntPtr camera), 
            nameof(CameraService.gp_camera_new)
        );

        _validator.Validate(
            CameraService.gp_camera_init(camera, context), 
            nameof(CameraService.gp_camera_init)
        );

        _manager.CameraContext = new CameraContext(context, camera);

        _dispatcher.Dispatch(new CameraConnected());

        return Task.FromResult(true);
    }
}