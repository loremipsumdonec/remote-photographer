
using Boilerplate.Features.Reactive.Events;

namespace RemotePhotographer.Features.Photographer.Events;

public class ImageCaptured
    : Event
{
    public ImageCaptured()
    {
    }

    public ImageCaptured(string path, byte[] data, IEnumerable<string> tags)
    {
        Path = path;
        Data = data;
        Tags = new List<string>(tags);
    }

    public string Path { get; set; }

    public byte[] Data { get; set; }

    public IEnumerable<string> Tags {get; set;}
}