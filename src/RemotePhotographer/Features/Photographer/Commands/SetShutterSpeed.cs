using Boilerplate.Features.Core.Commands;

namespace RemotePhotographer.Features.Photographer.Commands;

public class SetShutterSpeed
    : Command
{
    public SetShutterSpeed()
    {
    }

    public SetShutterSpeed(string value)
    {
        Value = value;
    }

    public string Value { get; set; }
}