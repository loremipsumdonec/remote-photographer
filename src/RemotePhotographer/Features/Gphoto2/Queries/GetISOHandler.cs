using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;
using Boilerplate.Features.Core.Services;
using RemotePhotographer.Features.Gphoto2.Services;
using RemotePhotographer.Features.Gphoto2.Services.Interop;
using RemotePhotographer.Features.Photographer.Models;
using RemotePhotographer.Features.Photographer.Queries;

namespace RemotePhotographer.Features.Gphoto2.Queries;

[Handle(typeof(GetISO))]
public class GetISOHandler
    : QueryHandler<GetISO>
{
    private readonly IModelService _service;
    private readonly ICameraContextManager _manager;

    public GetISOHandler(IModelService service, ICameraContextManager manager) 
    {
        _service = service;
        _manager = manager;
    }

    public override async Task<IModel> ExecuteAsync(GetISO query)
    {
        var isoStatus = CameraService.gp_camera_get_single_config(
            _manager.CameraContext.Camera, "iso", out IntPtr widget, _manager.CameraContext.Context
        );

        var model = await _service.CreateModelAsync<ISO>(widget);

        return model;
    }
}