
using Boilerplate.Features.Reactive.Events;

namespace RemotePhotographer.Features.Photographer.Events;

public class ApertureChanged
    : Event
{
    public ApertureChanged()
    {
    }

    public ApertureChanged(string value)
    {
        Value = value;
    }

    public string Value { get; set; }
}