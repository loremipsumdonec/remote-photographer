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

    public Task<bool> Disconnect(string cameraId, [Service] ICommandDispatcher dispatcher) 
    {
        return dispatcher.DispatchAsync(
            new DisconnectCamera(cameraId)
        );
    }

    public Task<bool> Aperture(string cameraId, string value, [Service] ICommandDispatcher dispatcher)
    {
        return dispatcher.DispatchAsync(
            new SetAperture(cameraId, value)
        );
    }

    public Task<bool> Iso(string cameraId, string value, [Service] ICommandDispatcher dispatcher)
    {
        return dispatcher.DispatchAsync(
            new SetISO(cameraId, value)
        );
    }

    public Task<bool> ShutterSpeed(string cameraId, string value, [Service] ICommandDispatcher dispatcher)
    {
        return dispatcher.DispatchAsync(
            new SetShutterSpeed(cameraId, value)
        );
    }

    public Task<bool> CaptureTarget(string cameraId, string value, [Service] ICommandDispatcher dispatcher)
    {
        return dispatcher.DispatchAsync(
            new SetCaptureTarget(cameraId, value)
        );
    }

    public Task<bool> ImageFormat(string cameraId, string value, [Service] ICommandDispatcher dispatcher)
    {
        return dispatcher.DispatchAsync(
            new SetImageFormat(cameraId, value)
        );
    }

    public Task<bool> CaptureImage(string cameraId, [Service] ICommandDispatcher dispatcher)
    {
        return dispatcher.DispatchAsync(
            new CaptureImage(cameraId)
        );
    }

    public Task<bool> StartPreview(string cameraId, int fps, [Service] ICommandDispatcher dispatcher)
    {
        return dispatcher.DispatchAsync(
            new StartPreview(cameraId, fps)
        );
    }

    public Task<bool> StopPreview(string cameraId, [Service] ICommandDispatcher dispatcher)
    {
        return dispatcher.DispatchAsync(
            new StopPreview(cameraId)
        );
    }
    
    public Task<bool> StartRecoding(string cameraId, int fps, [Service] ICommandDispatcher dispatcher)
    {
        return dispatcher.DispatchAsync(
            new StartRecording(cameraId, fps)
        );
    }

    public Task<bool> StopRecording(string cameraId, [Service] ICommandDispatcher dispatcher)
    {
        return dispatcher.DispatchAsync(
            new StopRecording(cameraId)
        );
    }

    public Task<bool> StartSession(string cameraId, Session session, [Service] ICommandDispatcher dispatcher)
    {
        return dispatcher.DispatchAsync(
            new StartSession(cameraId, session)
        );
    }

    public Task<bool> StopSession(string cameraId, [Service] ICommandDispatcher dispatcher)
    {
        return dispatcher.DispatchAsync(
            new StopSession(cameraId)
        );
    }
}