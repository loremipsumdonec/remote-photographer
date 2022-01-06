using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;
using RemotePhotographer.Features.Photographer.Models;

namespace RemotePhotographer.Features.Photographer.Queries;

public class GetFolders 
    : Query
{
    public GetFolders()
    {
    }

    public GetFolders(string path)
    {
        Path = path;
    }

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