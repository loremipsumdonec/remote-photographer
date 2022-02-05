
using Boilerplate.Features.Reactive.Events;

namespace RemotePhotographer.Features.Photographer.Events;

public class VideoImageCaptured
    : Event
{
    public VideoImageCaptured()
    {
    }

    public VideoImageCaptured(byte[] data, IEnumerable<string> tags)
    {
        Data = data;
        Tags = new List<string>(tags);
    }

    public byte[] Data { get; set; }

    public IEnumerable<string> Tags { get; set;}
}