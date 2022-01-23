using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using RemotePhotographer.Features.Gphoto2.Services.Interop;
using RemotePhotographer.Features.Gphoto2.Services;
using RemotePhotographer.Features.Photographer.Commands;
using Boilerplate.Features.Core.Services;
using RemotePhotographer.Features.Photographer.Models;
using System.Runtime.InteropServices;
using Boilerplate.Features.Reactive.Services;
using RemotePhotographer.Features.Photographer.Events;

namespace RemotePhotographer.Features.Gphoto2.Commands;

[Handle(typeof(SetCaptureTarget))]
public class SetCaptureTargetHandler
    : CommandHandler<SetCaptureTarget>
{
    private readonly IModelService _service;
    private readonly ICameraContextManager _manager;
    private readonly IMethodValidator _validator;
    private readonly IEventDispatcher _dispatcher;

    public SetCaptureTargetHandler(
        ICameraContextManager manager,
        IModelService service,
        IMethodValidator validator, 
        IEventDispatcher dispatcher)
    {
        _manager = manager;
        _service = service;
        _validator = validator;
        _dispatcher = dispatcher;
    }

    public override async Task<bool> ExecuteAsync(SetCaptureTarget command)
    {
        _validator.Validate(
            CameraService.gp_camera_get_single_config(
                _manager.CameraContext.Camera, "capturetarget", out IntPtr widget, _manager.CameraContext.Context
            ), 
            nameof(CameraService.gp_camera_get_single_config)
        );

        if(await ValidateAsync(widget, command)) 
        {
            IntPtr value = Marshal.StringToHGlobalAnsi(command.Value);

            _validator.Validate(
                WidgetService.gp_widget_set_value(widget, value), 
                nameof(WidgetService.gp_widget_set_value)
            );

            _validator.Validate(
               CameraService.gp_camera_set_single_config(
                    _manager.CameraContext.Camera, "capturetarget", widget, _manager.CameraContext.Context
                ), 
               nameof(CameraService.gp_camera_set_single_config)
            );

            Marshal.FreeHGlobal(value);

            _dispatcher.Dispatch(new CaptureTargetChanged(command.Value));
        } 
        else 
        {
            throw new ArgumentException($"Capture target value {command.Value} does not exists");
        }

        return true;
    }

    private async Task<bool> ValidateAsync(IntPtr widget, SetCaptureTarget command)
    {
        var model = await _service.CreateModelAsync<CaptureTarget>(widget);
        return model.Values.Contains(command.Value);
    }
}