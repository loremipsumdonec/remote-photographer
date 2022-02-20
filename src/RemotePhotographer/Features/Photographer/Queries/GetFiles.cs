using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;

namespace RemotePhotographer.Features.Photographer.Queries;

public class GetFiles 
    : Query
{
    public GetFiles()
    {
    }

    public GetFiles(string cameraId, string path)
    {
        CameraId = cameraId;
        Path = path;
    }

    public string CameraId { get; set; }

    public string Path { get; set; }
}

public class GetFilesModel 
    : IModel
{
    public IEnumerable<string> Files { get; } = new List<string>();

    public void Add(string file) 
    {
        ((List<string>)Files).Add(file);        
    }
}