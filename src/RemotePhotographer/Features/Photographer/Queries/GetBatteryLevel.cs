using Boilerplate.Features.Core.Queries;

namespace RemotePhotographer.Features.Photographer.Queries;
public class GetBatteryLevel 
    : Query
{
    public GetBatteryLevel(string cameraId)
    {
        CameraId = cameraId;
    }

    public string CameraId { get; set; }
}