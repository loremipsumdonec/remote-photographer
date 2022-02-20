using Boilerplate.Features.Core.Commands;

namespace RemotePhotographer.Features.Photographer.Commands;

public class SetAperture
    : Command
{
    public SetAperture()
    {
    }

    public SetAperture(string cameraId, string value)
    {
        CameraId = cameraId;
        Value = value;
    }

    public string CameraId { get; set; }

    public string Value { get; set; }
}