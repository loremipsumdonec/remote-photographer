using Boilerplate.Features.Core.Commands;

namespace RemotePhotographer.Features.Photographer.Commands;

public class SetCaptureTarget
    : Command
{
    public SetCaptureTarget()
    {
    }

    public SetCaptureTarget(string cameraId, string value)
    {
        cameraId = cameraId;
        Value = value;
    }

    public string CameraId { get; set; }

    public string Value { get; set; }
}