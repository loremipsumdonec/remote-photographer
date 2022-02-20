using Boilerplate.Features.Core.Commands;

namespace RemotePhotographer.Features.Photographer.Commands;

public class StopPreview
    : Command
{
    public StopPreview(string cameraId)
    {
        CameraId = cameraId;
    }

    public string CameraId { get; set; }
}