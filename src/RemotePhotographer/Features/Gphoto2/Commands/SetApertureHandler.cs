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

[Handle(typeof(SetAperture))]
public class SetApertureHandler
    : CommandHandler<SetAperture>
{
    private readonly ICameraContextManager _manager;
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly IMethodValidator _validator;
    private readonly IEventDispatcher _dispatcher;

    public SetApertureHandler(
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

    public override async Task<bool> ExecuteAsync(SetAperture command)
    {
        var aperture = await GetApertureAsync(command);

        if(aperture.Current == command.Value) 
        {
            return true;
        }

        if(Validate(command, aperture)) 
        {
            Set(command);
            _dispatcher.Dispatch(new ApertureChanged(command.Value));
        }
        else 
        {
            throw new ArgumentException($"Aperture value {command.Value} does not exists");
        }

        return true;
    }

    private Task<Aperture> GetApertureAsync(SetAperture command) 
    {
        return _queryDispatcher.DispatchAsync<Aperture>(new GetAperture(command.CameraId));
    }

    private bool Validate(SetAperture command, Aperture aperture)
    {
        return aperture.Values.Contains(command.Value);
    }

    private void Set(SetAperture command) 
    {
        lock(_manager.Door) 
        {
            _manager.EnsureCameraContext();

            
            _validator.Validate(
                CameraService.gp_camera_get_single_config(
                    _manager.CameraContext.Camera, "aperture", out IntPtr widget, _manager.CameraContext.Context
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
                    _manager.CameraContext.Camera, "aperture", widget, _manager.CameraContext.Context
                ), 
                nameof(CameraService.gp_camera_set_single_config)
            );

            Marshal.FreeHGlobal(value);
        }
    }
}