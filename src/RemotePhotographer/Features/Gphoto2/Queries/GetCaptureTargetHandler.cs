using System.Runtime.InteropServices;
using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;
using RemotePhotographer.Features.Gphoto2.Services;
using RemotePhotographer.Features.Gphoto2.Services.Interop;
using RemotePhotographer.Features.Photographer.Models;
using RemotePhotographer.Features.Photographer.Queries;

namespace RemotePhotographer.Features.Gphoto2.Queries;

[Handle(typeof(GetCaptureTarget))]
public class GetCaptureTargetHandler
    : QueryHandler<GetCaptureTarget>
{
    private readonly ICameraContextManager _manager;
    private readonly IMethodValidator _validator;

    public GetCaptureTargetHandler(
        ICameraContextManager manager, 
        IMethodValidator validator)
    {
        _manager = manager;
        _validator = validator;
    }

    public override Task<IModel> ExecuteAsync(GetCaptureTarget query)
    {
        lock(_manager.Door) 
        {
            _manager.EnsureCameraContext();
            
            _validator.Validate(CameraService.gp_camera_get_single_config(
                _manager.CameraContext.Camera, "capturetarget", out IntPtr widget, _manager.CameraContext.Context
            ), nameof(CameraService.gp_camera_get_single_config));

            var model = CreateCaptureTarget(widget);
        
            return Task.FromResult((IModel)model);
        }
    }

    private CaptureTarget CreateCaptureTarget(IntPtr widget)
    {
        var model = new CaptureTarget(); 

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