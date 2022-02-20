using Boilerplate.Features.Core.Queries;

namespace RemotePhotographer.Features.Photographer.Queries;
public class GetCaptureTarget 
    : Query
{
    public GetCaptureTarget(string cameraId)
    {
        CameraId = cameraId;
    }

    public string CameraId { get; set; }
}