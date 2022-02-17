
using Boilerplate.Features.Reactive.Events;
using MassTransit;

namespace RemotePhotographer.Features.Photographer.Events;

public class PreviewImageCaptured
    : Event
{
    public PreviewImageCaptured()
    {
    }

    public PreviewImageCaptured(MessageData<byte[]> data, IEnumerable<string> tags)
    {
        Data = data;
        Tags = new List<string>(tags);
    }

    public MessageData<byte[]> Data { get; set; }

    public IEnumerable<string> Tags { get; set;}
}