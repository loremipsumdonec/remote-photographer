using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;
using RemotePhotographer.Features.Photographer.Models;

namespace RemotePhotographer.Features.Photographer.Queries;
public class GetConfigs 
    : Query
{
    public GetConfigs(string cameraId)
    {
        CameraId = cameraId;
    }

    public string CameraId { get; set; }
}

public class GetConfigsModel 
    : IModel
{
    public IEnumerable<Config> Configs { get; } = new List<Config>();

    public void Add(Config config) 
    {
        ((List<Config>)Configs).Add(config);
    }
}