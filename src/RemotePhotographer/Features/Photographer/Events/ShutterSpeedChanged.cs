
using Boilerplate.Features.Reactive.Events;

namespace RemotePhotographer.Features.Photographer.Events;

public class ShutterSpeedChanged
    : Event
{
    public ShutterSpeedChanged()
    {
    }

    public ShutterSpeedChanged(string value)
    {
        Value = value;
    }

    public string Value { get; set; }
}