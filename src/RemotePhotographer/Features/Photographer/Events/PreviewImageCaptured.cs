
using Boilerplate.Features.Reactive.Events;

namespace RemotePhotographer.Features.Photographer.Events;

public class PreviewImageCaptured
    : Event
{
    public PreviewImageCaptured()
    {
    }

    public PreviewImageCaptured(byte[] data)
    {
        Data = data;
    }

    public byte[] Data { get; set; }
}