using Boilerplate.Features.Core.Commands;

namespace RemotePhotographer.Features.Photographer.Commands;

public class SetISO
    : Command
{
    public SetISO()
    {
    }

    public SetISO(string value)
    {
        Value = value;
    }

    public string Value { get; set; }
}