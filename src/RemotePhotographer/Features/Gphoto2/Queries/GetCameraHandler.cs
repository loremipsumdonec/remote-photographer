using System.Runtime.InteropServices;
using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;
using RemotePhotographer.Features.Gphoto2.Extensions;
using RemotePhotographer.Features.Gphoto2.Models;
using RemotePhotographer.Features.Gphoto2.Services;
using RemotePhotographer.Features.Gphoto2.Services.Interop;
using RemotePhotographer.Features.Photographer.Models;
using RemotePhotographer.Features.Photographer.Queries;

namespace RemotePhotographer.Features.Gphoto2.Queries;

[Handle(typeof(GetCamera))]
public class GetCameraHandler
    : QueryHandler<GetCamera>
{
    private readonly ICameraContextManager _manager;
    private readonly IMethodValidator _validator;

    public GetCameraHandler(ICameraContextManager manager, IMethodValidator validator)
    {
        _manager = manager;
        _validator = validator;        
    }

    public override Task<IModel> ExecuteAsync(GetCamera query)
    {
        var model = new Camera();
        LoadCameraId(model);
        LoadAbilities(model);
        LoadAbout(model);
        LoadSummary(model);

        return Task.FromResult((IModel)model);
    }

    private void LoadCameraId(Camera model) 
    {
        lock(_manager.Door) 
        {
            _manager.EnsureCameraContext();

            _validator.Validate(CameraService.gp_camera_get_single_config(
                _manager.CameraContext.Camera, "eosserialnumber", out IntPtr widget, _manager.CameraContext.Context
            ), nameof(CameraService.gp_camera_get_single_config));

            _validator.Validate(
                WidgetService.gp_widget_get_value(widget, out IntPtr valuePointer), 
                nameof(WidgetService.gp_widget_get_value)
            );

            model.CameraId = Marshal.PtrToStringAnsi(valuePointer);
        }
    }

    private void LoadAbilities(Camera model) 
    {
        lock(_manager.Door) 
        {
            _manager.EnsureCameraContext();

             _validator.Validate(
                CameraService.gp_camera_get_abilities(
                    _manager.CameraContext.Camera,
                    out CameraAbilities abilities
                ),
                nameof(CameraService.gp_camera_get_abilities)
            );

            model.Name = abilities.model.ConvertToString();
        }
    }

    private void LoadAbout(Camera model)
    {
        lock(_manager.Door) 
        {
            _manager.EnsureCameraContext();

            _validator.Validate(
                CameraService.gp_camera_get_about(
                    _manager.CameraContext.Camera,
                    out CameraText text,
                    _manager.CameraContext.Context
                ),
                nameof(CameraService.gp_camera_get_about)
            );

            model.About = text.text.ConvertToString();
        }
    }

    private void LoadSummary(Camera model)
    {
        lock(_manager.Door) 
        {
            _manager.EnsureCameraContext();
            
            _validator.Validate(
                CameraService.gp_camera_get_summary(
                    _manager.CameraContext.Camera,
                    out CameraText text,
                    _manager.CameraContext.Context
                ),
                nameof(CameraService.gp_camera_get_summary)
            );

            model.Summary = text.text.ConvertToString();
        }
    }
}