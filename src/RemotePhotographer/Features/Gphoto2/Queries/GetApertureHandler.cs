using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;
using Boilerplate.Features.Core.Services;
using RemotePhotographer.Features.Gphoto2.Services;
using RemotePhotographer.Features.Gphoto2.Services.Interop;
using RemotePhotographer.Features.Photographer.Models;
using RemotePhotographer.Features.Photographer.Queries;

namespace RemotePhotographer.Features.Gphoto2.Queries;

[Handle(typeof(GetAperture))]
public class GetApertureHandler
    : QueryHandler<GetAperture>
{
    private readonly IModelService _service;
    private readonly ICameraContextManager _manager;

    public GetApertureHandler(IModelService service, ICameraContextManager manager) 
    {
        _service = service;
        _manager = manager;
    }

    public override async Task<IModel> ExecuteAsync(GetAperture query)
    {
        var isoStatus = CameraService.gp_camera_get_single_config(
            _manager.CameraContext.Camera, "aperture", out IntPtr widget, _manager.CameraContext.Context
        );

        var model = await _service.CreateModelAsync<Aperture>(widget);

        return model;
    }
}