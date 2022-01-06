
using Boilerplate.Features.Reactive.Events;

namespace RemotePhotographer.Features.Photographer.Events;

public class ImageCaptured
    : Event
{
    public ImageCaptured()
    {
    }

    public ImageCaptured(string path)
    {
        Path = path;
    }

    public string Path { get; set; }
}