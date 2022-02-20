using Boilerplate.Features.Core.Queries;

namespace RemotePhotographer.Features.Photographer.Queries;

public class GetShutterSpeed 
    : Query
{
    public GetShutterSpeed(string cameraId)
    {
        CameraId = cameraId;
    }

    public string CameraId { get; set; }
}