using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using RemotePhotographer.Features.Gphoto2.Services;
using RemotePhotographer.Features.Photographer.Commands;
using Boilerplate.Features.Reactive.Services;

namespace RemotePhotographer.Features.Gphoto2.Commands;

[Handle(typeof(StartCapturePreview))]
public class StartCapturePreviewHandler
    : CommandHandler<StartCapturePreview>
{
    private readonly ICameraContextManager _manager;
    private readonly IMethodValidator _validator;
    private readonly IEventDispatcher _dispatcher;
    private readonly CapturePreviewBackgroundService _service;

    public StartCapturePreviewHandler(
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

    public override async Task<bool> ExecuteAsync(StartCapturePreview command)
    {
        await _service.StartPreviewAsync(command.FPS);
        return true;
    }
}