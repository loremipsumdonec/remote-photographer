using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using RemotePhotographer.Features.Gphoto2.Services;
using RemotePhotographer.Features.Photographer.Commands;
using Boilerplate.Features.Reactive.Services;
using RemotePhotographer.Features.Gphoto2.Services.Interop;
using System.Runtime.InteropServices;

namespace RemotePhotographer.Features.Gphoto2.Commands;

[Handle(typeof(StopCapturePreview))]
public class StopCapturePreviewHandler
    : CommandHandler<StopCapturePreview>
{
    private readonly ICameraContextManager _manager;
    private readonly IMethodValidator _validator;
    private readonly IEventDispatcher _dispatcher;
    private readonly CapturePreviewBackgroundService _service;

    public StopCapturePreviewHandler(
        ICameraContextManager manager,
        IMethodValidator validator,
        IEventDispatcher dispatcher, 
        CapturePreviewBackgroundService service)
    {
        _manager = manager;
        _validator = validator;
        _dispatcher = dispatcher;
        _service = service;
    }

    public override async Task<bool> ExecuteAsync(StopCapturePreview command)
    {
        await _service.StopPreviewAsync();        
        return true;
    }

    private void CloseViewFinder() 
    {
        lock(_manager.Door) 
        {
            _manager.EnsureCameraContext();

            _validator.Validate(
                CameraService.gp_camera_get_single_config(
                    _manager.CameraContext.Camera, "viewfinder", out IntPtr widget, _manager.CameraContext.Context
                ), 
                nameof(CameraService.gp_camera_get_single_config)
            );

            IntPtr value = Marshal.StringToHGlobalAnsi("0");

            _validator.Validate(
                WidgetService.gp_widget_set_value(widget, value), 
                nameof(WidgetService.gp_widget_set_value)
            );

            _validator.Validate(
                CameraService.gp_camera_set_single_config(
                    _manager.CameraContext.Camera, "viewfinder", widget, _manager.CameraContext.Context
                ), 
                nameof(CameraService.gp_camera_set_single_config)
            );

            Marshal.FreeHGlobal(value);
        }
    }
}