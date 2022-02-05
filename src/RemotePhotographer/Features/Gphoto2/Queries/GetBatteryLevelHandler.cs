using System.Runtime.InteropServices;
using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;
using RemotePhotographer.Features.Gphoto2.Services;
using RemotePhotographer.Features.Gphoto2.Services.Interop;
using RemotePhotographer.Features.Photographer.Models;
using RemotePhotographer.Features.Photographer.Queries;

namespace RemotePhotographer.Features.Gphoto2.Queries;

[Handle(typeof(GetBatteryLevel))]
public class GetBatteryLevelHandler
    : QueryHandler<GetBatteryLevel>
{
    private readonly ICameraContextManager _manager;
    private readonly IMethodValidator _validator;

    public GetBatteryLevelHandler(
        ICameraContextManager manager, 
        IMethodValidator validator)
    {
        _manager = manager;
        _validator = validator;
    }

    public override Task<IModel> ExecuteAsync(GetBatteryLevel query)
    {
        lock(_manager.Door) 
        {
            _manager.EnsureCameraContext();
            
            _validator.Validate(CameraService.gp_camera_get_single_config(
                _manager.CameraContext.Camera, "batterylevel", out IntPtr widget, _manager.CameraContext.Context
            ), nameof(CameraService.gp_camera_get_single_config));

            var model = CreateBatteryLevel(widget);
        
            return Task.FromResult((IModel)model);
        }
    }

    private BatteryLevel CreateBatteryLevel(IntPtr widget)
    {
        var model = new BatteryLevel(); 

        _validator.Validate(
            WidgetService.gp_widget_get_value(widget, out IntPtr valuePointer), 
            nameof(WidgetService.gp_widget_get_value)
        );

        string batteryLevelAsText =  Marshal.PtrToStringAnsi(valuePointer);
        model.Percent = int.Parse(batteryLevelAsText.Substring(0, batteryLevelAsText.Length - 1));

        return model;
    }
}