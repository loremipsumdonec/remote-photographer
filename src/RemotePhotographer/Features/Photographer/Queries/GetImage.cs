using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;
namespace RemotePhotographer.Features.Photographer.Queries;

public class GetImage 
    : Query
{
    public GetImage()
    {
    }

    public GetImage(string cameraId, string path)
    {
        CameraId = cameraId;
        Path = path;
    }

    public string CameraId { get; set; }

    public string Path { get; set; }
}

public class GetImageModel 
    : IModel
{
    public string Path {get; set;}

    public Byte[] Data {get; set;}
}