using Boilerplate.Features.Core.Commands;

namespace RemotePhotographer.Features.Photographer.Commands;

public class StartPreview
    : Command
{
    public StartPreview(string cameraId, int fps)
    {
        CameraId = cameraId;
        FPS = fps;
    }

    public string CameraId { get; set; }

    public int FPS { get; set; }
}