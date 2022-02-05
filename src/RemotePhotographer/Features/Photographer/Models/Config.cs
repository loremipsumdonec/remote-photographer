using Boilerplate.Features.Core;

namespace RemotePhotographer.Features.Photographer.Models;

public class Config
    : IModel
{
    public string ConfigId { get; set;}

    public string Name { get; set; }

    public string Type { get; set; }

    public string Current { get; set; }

    public IEnumerable<string> Values {get; } = new List<string>();

    public void Add(string value)
    {
        ((List<string>)Values).Add(value);
    }
}