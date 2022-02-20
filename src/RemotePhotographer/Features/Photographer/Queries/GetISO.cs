using Boilerplate.Features.Core.Queries;

namespace RemotePhotographer.Features.Photographer.Queries;

public class GetISO 
    : Query
{
    public GetISO(string cameraId)
    {
        CameraId = cameraId;
    }
    
    public string CameraId { get; set; }

}