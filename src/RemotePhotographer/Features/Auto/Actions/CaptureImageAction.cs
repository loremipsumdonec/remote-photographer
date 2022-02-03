
namespace RemotePhotographer.Features.Auto.Actions;

public class CaptureImageAction
    : IAction
{
    public string Aperture { get; set; }

    public string ShutterSpeed { get; set; }

    public string ISO { get; set; }

    public string ImageFormat { get; set; }

    public int Exposures { get; set;}
}