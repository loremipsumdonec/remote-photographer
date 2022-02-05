using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using RemotePhotographer.Features.Gphoto2.Services;
using RemotePhotographer.Features.Photographer.Commands;

namespace RemotePhotographer.Features.Gphoto2.Commands;

[Handle(typeof(StartRecording))]
public class StartRecordingHandler
    : CommandHandler<StartRecording>
{
    private readonly ICameraContextManager _manager;
    private readonly IRecordingService _service;

    public StartRecordingHandler(
        ICameraContextManager manager,
        IRecordingService service)
    {
        _manager = manager;
        _service = service;
    }

    public override async Task<bool> ExecuteAsync(StartRecording command)
    {
        lock(_manager.Door) 
        {
            _manager.EnsureCameraContext();
        }

        await _service.StartRecodingAsync(command.FPS);
        return true;
    }
}