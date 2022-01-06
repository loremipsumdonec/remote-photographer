using Boilerplate.Features.Core;

namespace RemotePhotographer.Features.Photographer.Models;

public class Camera
    : IModel
{
    public Camera(string name, string portName)
    {
        Name = name;
        PortName = portName;
    }

    public string Name { get; set; }      

    public string PortName { get; set; }
}