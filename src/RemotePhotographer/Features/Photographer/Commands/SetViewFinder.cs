using Boilerplate.Features.Core.Commands;

namespace RemotePhotographer.Features.Photographer.Commands;

public class SetViewFinder
    : Command
{
    public SetViewFinder()
    {
    }

    public SetViewFinder(bool open)
    {
        Open = open;
    }

    public bool Open { get; set; }
}