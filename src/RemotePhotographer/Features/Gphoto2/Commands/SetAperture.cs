using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using RemotePhotographer.Features.Gphoto2.Services.Interop;
using RemotePhotographer.Features.Gphoto2.Services;
using RemotePhotographer.Features.Photographer.Commands;
using Boilerplate.Features.Core.Services;
using RemotePhotographer.Features.Photographer.Models;
using System.Runtime.InteropServices;

namespace RemotePhotographer.Features.Gphoto2.Commands;

[Handle(typeof(SetAperture))]
public class SetApertureHandler
    : CommandHandler<SetAperture>
{
    private readonly IModelService _service;
    private readonly ICameraContextManager _manager;
    private readonly IMethodValidator _validator;

    public SetApertureHandler(
        ICameraContextManager manager, 
        IModelService service, 
        IMethodValidator validator)
    {
        _manager = manager;
        _service = service;
        _validator = validator;
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