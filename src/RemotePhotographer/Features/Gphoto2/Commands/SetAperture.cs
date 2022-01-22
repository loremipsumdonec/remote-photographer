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

[Handle(typeof(SetAperture))]
public class SetApertureHandler
    : CommandHandler<SetAperture>
{
    private readonly IModelService _service;
    private readonly ICameraContextManager _manager;
    private readonly IMethodValidator _validator;
    private readonly IEventDispatcher _dispatcher;

    public SetApertureHandler(
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

    public override async Task<bool> ExecuteAsync(SetAperture command)
    {
        _validator.Validate(
            CameraService.gp_camera_get_single_config(
                _manager.CameraContext.Camera, "aperture", out IntPtr widget, _manager.CameraContext.Context
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
                    _manager.CameraContext.Camera, "aperture", widget, _manager.CameraContext.Context
                ), 
               nameof(CameraService.gp_camera_set_single_config)
            );

            Marshal.FreeHGlobal(value);

            _dispatcher.Dispatch(new ApertureChanged(command.Value));
        } 
        else 
        {
            throw new ArgumentException($"Aperture value {command.Value} does not exists");
        }

        return true;
    }

    private async Task<bool> ValidateAsync(IntPtr widget, SetAperture command)
    {
        var model = await _service.CreateModelAsync<Aperture>(widget);
        return model.Values.Contains(command.Value);
    }
}