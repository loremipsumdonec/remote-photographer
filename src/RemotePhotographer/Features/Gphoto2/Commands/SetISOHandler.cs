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

[Handle(typeof(SetISO))]
public class SetISOHandler
    : CommandHandler<SetISO>
{
    private readonly ICameraContextManager _manager;
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly IMethodValidator _validator;
    private readonly IEventDispatcher _dispatcher;

    public SetISOHandler(
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

    public override async Task<bool> ExecuteAsync(SetISO command)
    {
        var iso = await GetISOAsync();

        if(iso.Current == command.Value) 
        {
            return true;
        }

        if(Validate(command, iso)) 
        {
            Set(command);
            _dispatcher.Dispatch(new ISOChanged(command.Value));
        }
        else 
        {
            throw new ArgumentException($"Capture target value {command.Value} does not exists");
        }

        return true;
    }

    private Task<ISO> GetISOAsync() 
    {
        return _queryDispatcher.DispatchAsync<ISO>(new GetISO());
    }

    private bool Validate(SetISO command, ISO iso)
    {
        return iso.Values.Contains(command.Value);
    }

    private void Set(SetISO command) 
    {
        lock(_manager.Door) 
        {
            _manager.EnsureCameraContext();

            _validator.Validate(
                CameraService.gp_camera_get_single_config(
                    _manager.CameraContext.Camera, "iso", out IntPtr widget, _manager.CameraContext.Context
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
                    _manager.CameraContext.Camera, "iso", widget, _manager.CameraContext.Context
                ), 
                nameof(CameraService.gp_camera_set_single_config)
            );

            Marshal.FreeHGlobal(value);
        }
    }
}