using System.Runtime.InteropServices;
using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;
using RemotePhotographer.Features.Gphoto2.Services;
using RemotePhotographer.Features.Gphoto2.Services.Interop;
using RemotePhotographer.Features.Photographer.Models;
using RemotePhotographer.Features.Photographer.Queries;

namespace RemotePhotographer.Features.Gphoto2.Queries;

[Handle(typeof(GetAperture))]
public class GetApertureHandler
    : QueryHandler<GetAperture>
{
    private readonly ICameraContextManager _manager;
    private readonly IMethodValidator _validator;

    public GetApertureHandler(
        ICameraContextManager manager, 
        IMethodValidator validator)
    {
        _manager = manager;
        _validator = validator;
    }

    public override Task<IModel> ExecuteAsync(GetAperture query)
    {
        lock(_manager.Door) 
        {
            _manager.EnsureCameraContext();
            
            _validator.Validate(CameraService.gp_camera_get_single_config(
                _manager.CameraContext.Camera, "aperture", out IntPtr widget, _manager.CameraContext.Context
            ), nameof(CameraService.gp_camera_get_single_config));

            var model = CreateAperture(widget);
        
            return Task.FromResult((IModel)model);
        }
    }

    private Aperture CreateAperture(IntPtr widget)
    {
        var model = new Aperture(); 

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