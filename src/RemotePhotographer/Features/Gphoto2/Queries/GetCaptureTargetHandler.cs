using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;
using Boilerplate.Features.Core.Services;
using RemotePhotographer.Features.Gphoto2.Services;
using RemotePhotographer.Features.Gphoto2.Services.Interop;
using RemotePhotographer.Features.Photographer.Models;
using RemotePhotographer.Features.Photographer.Queries;

namespace RemotePhotographer.Features.Gphoto2.Queries;

[Handle(typeof(GetCaptureTarget))]
public class GetCaptureTargetHandler
    : QueryHandler<GetCaptureTarget>
{
    private readonly IModelService _service;
    private readonly ICameraContextManager _manager;
    private readonly IMethodValidator _validator;

    public GetCaptureTargetHandler(
        IModelService service, 
        ICameraContextManager manager, 
        IMethodValidator validator)
    {
        _service = service;
        _manager = manager;
        _validator = validator;
    }

    public override async Task<IModel> ExecuteAsync(GetCaptureTarget query)
    {
        _validator.Validate(CameraService.gp_camera_get_single_config(
            _manager.CameraContext.Camera, "capturetarget", out IntPtr widget, _manager.CameraContext.Context
        ), nameof(CameraService.gp_camera_get_single_config));

        var model = await _service.CreateModelAsync<CaptureTarget>(widget);

        return model;
    }
}