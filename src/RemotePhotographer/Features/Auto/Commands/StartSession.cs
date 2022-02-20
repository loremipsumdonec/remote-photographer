using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using RemotePhotographer.Features.Auto.Services;

namespace RemotePhotographer.Features.Auto.Models;

public class StartSession
    : Command
{
    public StartSession(string cameraId, Session session) 
    {
        CameraId = cameraId;
        Session = session;        
    }

    public string CameraId { get; set; }

    public Session Session { get; set;}
}

[Handle(typeof(StartSession))]
public class StartSessionHandler
    : CommandHandler<StartSession>
{
    private readonly SessionBackgroundService _service;

    public StartSessionHandler(SessionBackgroundService service) 
    {
        _service = service;
    }

    public override Task<bool> ExecuteAsync(StartSession command)
    {
        _service.StartSessionAsync(command.CameraId, command.Session);
        return Task.FromResult(true);
    }
}