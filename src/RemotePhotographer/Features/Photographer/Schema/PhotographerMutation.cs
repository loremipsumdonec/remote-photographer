using Boilerplate.Features.Core.Commands;
using RemotePhotographer.Features.Photographer.Commands;

namespace RemotePhotographer.Features.Photographer.Schema;

public class PhotographerMutation
{
    public Task<bool> Connect([Service] ICommandDispatcher dispatcher) 
    {
        return dispatcher.DispatchAsync(
            new ConnectCamera()
        );
    }

    public Task<bool> Disconnect([Service] ICommandDispatcher dispatcher) 
    {
        return dispatcher.DispatchAsync(
            new DisconnectCamera()
        );
    }

    public Task<bool> SetIso(string value, [Service] ICommandDispatcher dispatcher)
    {
        return dispatcher.DispatchAsync(
            new SetISO(value)
        );
    }

    public Task<bool> SetShutterSpeed(string value, [Service] ICommandDispatcher dispatcher)
    {
        return dispatcher.DispatchAsync(
            new SetShutterSpeed(value)
        );
    }

    public Task<bool> SetCaptureTarget(string value, [Service] ICommandDispatcher dispatcher)
    {
        return dispatcher.DispatchAsync(
            new SetCaptureTarget(value)
        );
    }

    public Task<bool> CaptureImage([Service] ICommandDispatcher dispatcher)
    {
        return dispatcher.DispatchAsync(
            new CaptureImage()
        );
    }
}