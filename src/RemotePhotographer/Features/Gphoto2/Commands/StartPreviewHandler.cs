using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using RemotePhotographer.Features.Gphoto2.Services;
using RemotePhotographer.Features.Photographer.Commands;

namespace RemotePhotographer.Features.Gphoto2.Commands;

[Handle(typeof(StartPreview))]
public class StartPreviewHandler
    : CommandHandler<StartPreview>
{
    private readonly ICameraContextManager _manager;
    private readonly IPreviewService _service;

    public StartPreviewHandler(
        ICameraContextManager manager,
        IPreviewService service)
    {
        _manager = manager;
        _service = service;
    }

    public override async Task<bool> ExecuteAsync(StartPreview command)
    {
        lock(_manager.Door) 
        {
            _manager.EnsureCameraContext();
        }

        await _service.StartPreviewAsync(command.FPS);
        return true;
    }
}