using Boilerplate.Features.Core.Queries;

namespace RemotePhotographer.Features.Photographer.Queries;
public class GetAperture 
    : Query
{
    public GetAperture(string cameraId)
    {
        CameraId = cameraId;
    }

    public string CameraId { get; set; }
}