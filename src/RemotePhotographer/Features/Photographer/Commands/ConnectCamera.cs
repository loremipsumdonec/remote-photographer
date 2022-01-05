using Boilerplate.Features.Core.Commands;

namespace RemotePhotographer.Features.Photographer.Commands;

public class ConnectCamera
    : Command
{
    public int ISO { get ; set; }

    public int Aperture {get; set;}

    public int ShutterSpeed {get; set;}
}