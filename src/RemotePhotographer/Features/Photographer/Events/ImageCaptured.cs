
using Boilerplate.Features.Reactive.Events;
using MassTransit;

namespace RemotePhotographer.Features.Photographer.Events;

public class ImageCaptured
    : Event
{
    public ImageCaptured()
    {
    }

    public ImageCaptured(string path, MessageData<byte[]> data, IEnumerable<string> tags)
    {
        Path = path;
        Data = data;
        Tags = new List<string>(tags);
    }

    public string Path { get; set; }

    public MessageData<byte[]> Data { get; set; }

    public IEnumerable<string> Tags {get; set;}
}