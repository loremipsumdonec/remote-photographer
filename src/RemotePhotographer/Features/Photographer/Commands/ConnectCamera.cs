using Boilerplate.Features.Core.Commands;

namespace RemotePhotographer.Features.Photographer.Commands;

public class ConnectCamera
    : Command
{
    public ConnectCamera(string cameraId, IEnumerable<string> tags)
    {
        CameraId = cameraId;
        Tags = new List<string>(tags);
    }

    public string CameraId { get; set; }

    public IEnumerable<string> Tags { get; set; }
}