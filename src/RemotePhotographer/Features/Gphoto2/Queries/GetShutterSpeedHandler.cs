using System.Runtime.InteropServices;
using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;
using RemotePhotographer.Features.Gphoto2.Services;
using RemotePhotographer.Features.Gphoto2.Services.Interop;
using RemotePhotographer.Features.Photographer.Models;
using RemotePhotographer.Features.Photographer.Queries;

namespace RemotePhotographer.Features.Gphoto2.Queries;

[Handle(typeof(GetShutterSpeed))]
public class GetShutterSpeedHandler
    : QueryHandler<GetShutterSpeed>
{
    private readonly ICameraContextManager _manager;
    private readonly IMethodValidator _validator;

    public GetShutterSpeedHandler(
        ICameraContextManager manager, 
        IMethodValidator validator)
    {
        _manager = manager;
        _validator = validator;
    }

    public override Task<IModel> ExecuteAsync(GetShutterSpeed query)
    {
        lock(_manager.Door) 
        {
            _manager.EnsureCameraContext();
            
            _validator.Validate(CameraService.gp_camera_get_single_config(
                _manager.CameraContext.Camera, "shutterspeed", out IntPtr widget, _manager.CameraContext.Context
            ), nameof(CameraService.gp_camera_get_single_config));

            var model = CreateShutterSpeed(widget);
        
            return Task.FromResult((IModel)model);
        }
    }

    private ShutterSpeed CreateShutterSpeed(IntPtr widget)
    {
        var model = new ShutterSpeed(); 

        _validator.Validate(
            WidgetService.gp_widget_get_value(widget, out IntPtr valuePointer), 
            nameof(WidgetService.gp_widget_get_value)
        );

        model.Current = Marshal.PtrToStringAnsi(valuePointer);

        int total = WidgetService.gp_widget_count_choices(widget);

        for(int index = 0; index < total; index++) 
        {
            _validator.Validate(
                WidgetService.gp_widget_get_choice(widget, index, out IntPtr choice),
                nameof(WidgetService.gp_widget_get_choice)
            );
            model.Add(Marshal.PtrToStringAnsi(choice));
        }

        return model;
    }
}