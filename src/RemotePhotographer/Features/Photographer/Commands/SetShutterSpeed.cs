using Boilerplate.Features.Core.Commands;

namespace RemotePhotographer.Features.Photographer.Commands;

public class SetShutterSpeed
    : Command
{
    public SetShutterSpeed()
    {
    }

    public SetShutterSpeed(string cameraId, string value)
    {
        CameraId = cameraId;
        Value = value;
    }

    public string CameraId { get; set; }

    public string Value { get; set; }
}