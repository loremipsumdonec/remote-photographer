using Boilerplate.Features.Core.Commands;
using RemotePhotographer.Features.Auto.Models;
using RemotePhotographer.Features.Photographer.Commands;

namespace RemotePhotographer.Features.Photographer.Schema;

public class PhotographerMutation
{
    public Task<bool> Connect(string cameraId, IEnumerable<string> tags, [Service] ICommandDispatcher dispatcher) 
    {
        return dispatcher.DispatchAsync(
            new ConnectCamera(cameraId, tags)
        );
    }

    public Task<bool> Disconnect([Service] ICommandDispatcher dispatcher) 
    {
        return dispatcher.DispatchAsync(
            new DisconnectCamera()
        );
    }

    public Task<bool> ViewFinder(bool open, [Service] ICommandDispatcher dispatcher)
    {
        return dispatcher.DispatchAsync(
            new SetViewFinder(open)
        );
    }

    public Task<bool> Iso(string value, [Service] ICommandDispatcher dispatcher)
    {
        return dispatcher.DispatchAsync(
            new SetISO(value)
        );
    }

    public Task<bool> ShutterSpeed(string value, [Service] ICommandDispatcher dispatcher)
    {
        return dispatcher.DispatchAsync(
            new SetShutterSpeed(value)
        );
    }

    public Task<bool> CaptureTarget(string value, [Service] ICommandDispatcher dispatcher)
    {
        return dispatcher.DispatchAsync(
            new SetCaptureTarget(value)
        );
    }

    public Task<bool> ImageFormat(string value, [Service] ICommandDispatcher dispatcher)
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

    public Task<bool> StartPreview(int fps, [Service] ICommandDispatcher dispatcher)
    {
        return dispatcher.DispatchAsync(
            new StartPreview(fps)
        );
    }

    public Task<bool> StopPreview([Service] ICommandDispatcher dispatcher)
    {
        return dispatcher.DispatchAsync(
            new StopPreview()
        );
    }
    
    public Task<bool> StartRecoding(int fps, [Service] ICommandDispatcher dispatcher)
    {
        return dispatcher.DispatchAsync(
            new StartRecording(fps)
        );
    }

    public Task<bool> StopRecording([Service] ICommandDispatcher dispatcher)
    {
        return dispatcher.DispatchAsync(
            new StopRecording()
        );
    }

    public Task<bool> StartSession(Session session, [Service] ICommandDispatcher dispatcher)
    {
        return dispatcher.DispatchAsync(
            new StartSession(session)
        );
    }

    public Task<bool> StopSession([Service] ICommandDispatcher dispatcher)
    {
        return dispatcher.DispatchAsync(
            new StopSession()
        );
    }
}