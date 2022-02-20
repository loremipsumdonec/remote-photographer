using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using RemotePhotographer.Features.Gphoto2.Services.Interop;
using RemotePhotographer.Features.Gphoto2.Services;
using RemotePhotographer.Features.Photographer.Commands;
using RemotePhotographer.Features.Photographer.Models;
using System.Runtime.InteropServices;
using Boilerplate.Features.Reactive.Services;
using RemotePhotographer.Features.Photographer.Events;
using Boilerplate.Features.Core.Queries;
using RemotePhotographer.Features.Photographer.Queries;

namespace RemotePhotographer.Features.Gphoto2.Commands;

[Handle(typeof(SetCaptureTarget))]
public class SetCaptureTargetHandler
    : CommandHandler<SetCaptureTarget>
{
    private readonly ICameraContextManager _manager;
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly IMethodValidator _validator;
    private readonly IEventDispatcher _dispatcher;

    public SetCaptureTargetHandler(
        ICameraContextManager manager,
        IQueryDispatcher queryDispatcher,
        IMethodValidator validator, 
        IEventDispatcher dispatcher)
    {
        _manager = manager;
        _queryDispatcher = queryDispatcher;
        _validator = validator;
        _dispatcher = dispatcher;
    }

    public override async Task<bool> ExecuteAsync(SetCaptureTarget command)
    {
        var captureTarget = await GetCaptureTargetAsync(command);

        if(captureTarget.Current == command.Value) 
        {
            return true;
        }

        if(Validate(command, captureTarget)) 
        {
            Set(command);
            _dispatcher.Dispatch(new CaptureTargetChanged(command.Value));
        }
        else 
        {
            throw new ArgumentException($"Capture target value {command.Value} does not exists");
        }

        return true;
    }

    private Task<CaptureTarget> GetCaptureTargetAsync(SetCaptureTarget command) 
    {
        return _queryDispatcher.DispatchAsync<CaptureTarget>(new GetCaptureTarget(command.CameraId));
    }

    private bool Validate(SetCaptureTarget command, CaptureTarget captureTarget)
    {
        return captureTarget.Values.Contains(command.Value);
    }

    private void Set(SetCaptureTarget command) 
    {
        lock(_manager.Door) 
        {
            _manager.EnsureCameraContext();
            
            _validator.Validate(
                CameraService.gp_camera_get_single_config(
                    _manager.CameraContext.Camera, "capturetarget", out IntPtr widget, _manager.CameraContext.Context
                ), 
                nameof(CameraService.gp_camera_get_single_config)
            );

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
        }
    }
}