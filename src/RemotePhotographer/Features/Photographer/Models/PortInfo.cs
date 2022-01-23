using Boilerplate.Features.Core;

namespace RemotePhotographer.Features.Photographer.Models;

public class PortInfo
    : IModel
{
    public PortInfo()
    {
    }

    public string Name { get; set; }      

    public string Path { get; set; }

    public string Type { get; set; }
}