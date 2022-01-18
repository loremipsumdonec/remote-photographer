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
        if (_manager.CameraContext == null)
        {
            throw new InvalidOperationException("No camera connected");
        }

        var model = new Camera();
        var context = _manager.CameraContext;

        _validator.Validate(
            CameraService.gp_camera_get_abilities(
                context.Camera,
                out CameraAbilities abilities
            ),
            nameof(CameraService.gp_camera_get_abilities)
        );

        model.Name = abilities.model.ConvertToString();

        LoadAbout(model, context);
        LoadSummary(model, context);

        return Task.FromResult((IModel)model);
    }

    private void LoadAbout(Camera model, CameraContext context)
    {
        _validator.Validate(
            CameraService.gp_camera_get_about(
                context.Camera,
                out CameraText text,
                context.Context
            ),
            nameof(CameraService.gp_camera_get_about)
        );

        model.About = text.text.ConvertToString();
    }

    private void LoadSummary(Camera model, CameraContext context)
    {
        _validator.Validate(
            CameraService.gp_camera_get_summary(
                context.Camera,
                out CameraText text,
                context.Context
            ),
            nameof(CameraService.gp_camera_get_summary)
        );

        model.Summary = text.text.ConvertToString();
    }
}