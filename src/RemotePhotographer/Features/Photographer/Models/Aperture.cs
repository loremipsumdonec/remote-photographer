using Boilerplate.Features.Core;

namespace RemotePhotographer.Features.Photographer.Models;

public class Aperture
    : IModel
{
    public string Current { get; set; }

    public IEnumerable<string> Values {get; } = new List<string>();

    public void Add(string value)
    {
        ((List<string>)Values).Add(value);
    }
}