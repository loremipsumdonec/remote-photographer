using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using RemotePhotographer.Features.Auto.Services;
using RemotePhotographer.Features.Gphoto2.Services;

namespace RemotePhotographer.Features.Auto.Models;

public class StartSession
    : Command
{
    public StartSession(Session session) 
    {
        Session = session;        
    }

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
        _service.StartSessionAsync(command.Session);
        return Task.FromResult(true);
    }
}