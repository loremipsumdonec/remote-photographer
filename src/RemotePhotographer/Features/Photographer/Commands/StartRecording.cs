using Boilerplate.Features.Core.Commands;

namespace RemotePhotographer.Features.Photographer.Commands;

public class StartRecording
    : Command
{
    public StartRecording(string cameraId, int fps)
    {
        CameraId = cameraId;
        FPS = fps;
    }

    public string CameraId { get; set; }
    
    public int FPS { get; set; }
}