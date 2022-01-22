
using Boilerplate.Features.Reactive.Events;

namespace RemotePhotographer.Features.Photographer.Events;

public class ISOChanged
    : Event
{
    public ISOChanged()
    {
    }

    public ISOChanged(string value)
    {
        Value = value;
    }

    public string Value { get; set; }
}