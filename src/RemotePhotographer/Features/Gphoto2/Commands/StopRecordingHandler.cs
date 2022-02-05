using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using RemotePhotographer.Features.Gphoto2.Services;
using RemotePhotographer.Features.Photographer.Commands;

namespace RemotePhotographer.Features.Gphoto2.Commands;

[Handle(typeof(StopRecording))]
public class StopRecordingHandler
    : CommandHandler<StopRecording>
{
    private readonly IRecordingService _service;

    public StopRecordingHandler(IRecordingService service)
    {
        _service = service;
    }

    public override async Task<bool> ExecuteAsync(StopRecording command)
    {   
        await _service.StopRecodingAsync();     
        return true;
    }
}