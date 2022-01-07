
using Boilerplate.Features.Reactive.Events;

namespace RemotePhotographer.Features.Photographer.Events;

public class ImageCaptured
    : Event
{
    public ImageCaptured()
    {
    }

    public ImageCaptured(string path, byte[] data)
    {
        Path = path;
        Data = data;
    }

    public string Path { get; set; }

    public byte[] Data { get; set; }
}