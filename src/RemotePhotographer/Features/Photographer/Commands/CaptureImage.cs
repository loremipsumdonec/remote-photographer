using Boilerplate.Features.Core.Commands;

namespace RemotePhotographer.Features.Photographer.Commands;

public class CaptureImage
    : Command
{
    public CaptureImage(string cameraId)
    {
        CameraId = cameraId;
    }

    public string CameraId { get; set; }

}