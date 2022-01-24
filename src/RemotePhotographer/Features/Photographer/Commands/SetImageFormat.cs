using Boilerplate.Features.Core.Commands;

namespace RemotePhotographer.Features.Photographer.Commands;

public class SetImageFormat
    : Command
{
    public SetImageFormat()
    {
    }

    public SetImageFormat(string value)
    {
        Value = value;
    }

    public string Value { get; set; }
}