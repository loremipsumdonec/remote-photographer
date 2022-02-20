using Boilerplate.Features.Core.Commands;

namespace RemotePhotographer.Features.Photographer.Commands;

public class DisconnectCamera
    : Command
{
    public DisconnectCamera(string cameraId)
    {
        CameraId = cameraId;
    }

    public string CameraId { get; set; }
}