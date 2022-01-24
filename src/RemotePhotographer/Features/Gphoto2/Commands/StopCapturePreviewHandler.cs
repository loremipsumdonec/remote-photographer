using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using RemotePhotographer.Features.Gphoto2.Services;
using RemotePhotographer.Features.Photographer.Commands;
using Boilerplate.Features.Reactive.Services;

namespace RemotePhotographer.Features.Gphoto2.Commands;

[Handle(typeof(StopCapturePreview))]
public class StopCapturePreviewHandler
    : CommandHandler<StopCapturePreview>
{
    private readonly ICameraContextManager _manager;
    private readonly IMethodValidator _validator;
    private readonly IEventDispatcher _dispatcher;
    private readonly CapturePreviewBackgroundService _service;

    public StopCapturePreviewHandler(
        ICameraContextManager manager,
        IMethodValidator validator,
        IEventDispatcher dispatcher, 
        CapturePreviewBackgroundService service)
    {
        _manager = manager;
        _validator = validator;
        _dispatcher = dispatcher;
        _service = service;
    }

    public override async Task<bool> ExecuteAsync(StopCapturePreview command)
    {
        await _service.StopPreviewAsync();
        return true;
    }
}