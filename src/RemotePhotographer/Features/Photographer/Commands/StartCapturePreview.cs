using Boilerplate.Features.Core.Commands;

namespace RemotePhotographer.Features.Photographer.Commands;

public class StartCapturePreview
    : Command
{
    public StartCapturePreview(int fps)
    {
        FPS = fps;
    }

    public int FPS { get; set; }
}