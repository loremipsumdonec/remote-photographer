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

    public Task<bool> SetImageFormat(string value, [Service] ICommandDispatcher dispatcher)
    {
        return dispatcher.DispatchAsync(
            new SetImageFormat(value)
        );
    }

    public Task<bool> CaptureImage([Service] ICommandDispatcher dispatcher)
    {
        return dispatcher.DispatchAsync(
            new CaptureImage()
        );
    }

    public Task<bool> CapturePreviewImage([Service] ICommandDispatcher dispatcher)
    {
        return dispatcher.DispatchAsync(
            new CapturePreviewImage()
        );
    }

    public Task<bool> StartCapturePreview(int fps, [Service] ICommandDispatcher dispatcher)
    {
        return dispatcher.DispatchAsync(
            new StartCapturePreview(fps)
        );
    }

    public Task<bool> StopCapturePreview([Service] ICommandDispatcher dispatcher)
    {
        return dispatcher.DispatchAsync(
            new StopCapturePreview()
        );
    }
}