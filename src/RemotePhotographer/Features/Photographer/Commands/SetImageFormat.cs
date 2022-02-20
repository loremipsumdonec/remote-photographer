using Boilerplate.Features.Core.Commands;

namespace RemotePhotographer.Features.Photographer.Commands;

public class SetImageFormat
    : Command
{
    public SetImageFormat()
    {
    }

    public SetImageFormat(string cameraId, string value)
    {
        CameraId = cameraId;
        Value = value;
    }

    public string CameraId { get; set; }

    public string Value { get; set; }
}