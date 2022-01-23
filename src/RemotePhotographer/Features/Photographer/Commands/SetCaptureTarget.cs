using Boilerplate.Features.Core.Commands;

namespace RemotePhotographer.Features.Photographer.Commands;

public class SetCaptureTarget
    : Command
{
    public SetCaptureTarget()
    {
    }

    public SetCaptureTarget(string value)
    {
        Value = value;
    }

    public string Value { get; set; }
}