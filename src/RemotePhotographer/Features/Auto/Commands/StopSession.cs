using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using RemotePhotographer.Features.Auto.Services;

namespace RemotePhotographer.Features.Auto.Models;

public class StopSession
    : Command
{
    public StopSession(string cameraId)
    {
        CameraId = cameraId;
    }

    public string CameraId { get; set; }
}

[Handle(typeof(StopSession))]
public class StopSessionHandler
    : CommandHandler<StopSession>
{
    private readonly SessionBackgroundService _service;

    public StopSessionHandler(SessionBackgroundService service) 
    {
        _service = service;
    }

    public override async Task<bool> ExecuteAsync(StopSession command)
    {
        await _service.StopSessionAsync();
        return true;
    }
}