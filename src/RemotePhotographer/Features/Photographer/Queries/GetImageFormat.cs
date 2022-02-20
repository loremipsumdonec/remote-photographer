using Boilerplate.Features.Core.Queries;

namespace RemotePhotographer.Features.Photographer.Queries;
public class GetImageFormat 
    : Query
{
    public GetImageFormat(string cameraId)
    {
        CameraId = cameraId;
    }

    public string CameraId { get; set; }
}