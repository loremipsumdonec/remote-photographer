using Boilerplate.Features.Core.Commands;

namespace RemotePhotographer.Features.Photographer.Commands;

public class SetAperture
    : Command
{
    public SetAperture()
    {
    }

    public SetAperture(string value)
    {
        Value = value;
    }

    public string Value { get; set; }
}