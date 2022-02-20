using Boilerplate.Features.Core.Commands;

namespace RemotePhotographer.Features.Photographer.Commands;

public class StopRecording
    : Command
{
    public StopRecording(string cameraId)
    {
        CameraId = cameraId;
    }

    public string CameraId { get; set; }
}