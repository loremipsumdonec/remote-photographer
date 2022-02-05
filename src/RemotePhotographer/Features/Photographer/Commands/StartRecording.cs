using Boilerplate.Features.Core.Commands;

namespace RemotePhotographer.Features.Photographer.Commands;

public class StartRecording
    : Command
{
    public StartRecording(int fps)
    {
        FPS = fps;
    }

    public int FPS { get; set; }
}