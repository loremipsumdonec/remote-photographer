
using Boilerplate.Features.Reactive.Events;

namespace RemotePhotographer.Features.Photographer.Events;

public class CaptureTargetChanged
    : Event
{
    public CaptureTargetChanged()
    {
    }

    public CaptureTargetChanged(string value)
    {
        Value = value;
    }

    public string Value { get; set; }
}