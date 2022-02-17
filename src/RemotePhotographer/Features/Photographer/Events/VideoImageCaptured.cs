
using Boilerplate.Features.Reactive.Events;
using MassTransit;

namespace RemotePhotographer.Features.Photographer.Events;

public class VideoImageCaptured
    : Event
{
    public VideoImageCaptured()
    {
    }

    public VideoImageCaptured(MessageData<byte[]> data, IEnumerable<string> tags)
    {
        Data = data;
        Tags = new List<string>(tags);
    }

    public MessageData<byte[]> Data { get; set; }

    public IEnumerable<string> Tags { get; set;}
}