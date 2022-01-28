using Boilerplate.Features.Core.Commands;

namespace RemotePhotographer.Features.Photographer.Commands;

public class ConnectCamera
    : Command
{
    public ConnectCamera(IEnumerable<string> tags)
    {
        Tags = new List<string>(tags);
    }

    public IEnumerable<string> Tags { get; set; }
}