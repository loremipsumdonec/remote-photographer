using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;

namespace RemotePhotographer.Features.Photographer.Queries;

public class GetFolders 
    : Query
{
    public GetFolders()
    {
    }

    public GetFolders(string cameraId, string path)
    {
        CameraId = cameraId;
        Path = path;
    }

    public string CameraId { get; set; }

    public string Path { get; set; }
}

public class GetFoldersModel 
    : IModel
{
    public IEnumerable<string> Folders { get; } = new List<string>();

    public void Add(string folder) 
    {
        ((List<string>)Folders).Add(folder);        
    }
}