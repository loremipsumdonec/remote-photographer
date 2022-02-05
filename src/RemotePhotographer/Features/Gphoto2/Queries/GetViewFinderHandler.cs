using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;
using RemotePhotographer.Features.Gphoto2.Services;
using RemotePhotographer.Features.Gphoto2.Services.Interop;
using RemotePhotographer.Features.Photographer.Models;
using RemotePhotographer.Features.Photographer.Queries;

namespace RemotePhotographer.Features.Gphoto2.Queries;

[Handle(typeof(GetViewFinder))]
public class GetViewFinderHandler
    : QueryHandler<GetViewFinder>
{
    private readonly ICameraContextManager _manager;
    private readonly IMethodValidator _validator;

    public GetViewFinderHandler(
        ICameraContextManager manager, 
        IMethodValidator validator)
    {
        _manager = manager;
        _validator = validator;
    }

    public override Task<IModel> ExecuteAsync(GetViewFinder query)
    {
        lock(_manager.Door) 
        {
            _manager.EnsureCameraContext();
            
            _validator.Validate(CameraService.gp_camera_get_single_config(
                _manager.CameraContext.Camera, "viewfinder", out IntPtr widget, _manager.CameraContext.Context
            ), nameof(CameraService.gp_camera_get_single_config));

            var model = CreateViewFinder(widget);
        
            return Task.FromResult((IModel)model);
        }
    }

    private ViewFinder CreateViewFinder(IntPtr widget)
    {
        var model = new ViewFinder(); 

        _validator.Validate(
            WidgetService.gp_widget_get_value(widget, out IntPtr valuePointer), 
            nameof(WidgetService.gp_widget_get_value)
        );

        model.Open = 1 == (int)valuePointer;

        return model;
    }
}