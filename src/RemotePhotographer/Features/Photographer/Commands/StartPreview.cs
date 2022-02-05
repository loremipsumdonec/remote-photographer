using Boilerplate.Features.Core.Commands;

namespace RemotePhotographer.Features.Photographer.Commands;

public class StartPreview
    : Command
{
    public StartPreview(int fps)
    {
        FPS = fps;
    }

    public int FPS { get; set; }
}