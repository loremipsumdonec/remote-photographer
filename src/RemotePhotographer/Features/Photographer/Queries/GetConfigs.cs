using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;

namespace RemotePhotographer.Features.Photographer.Queries;
public class GetConfigs 
    : Query
{
}

public class GetConfigsModel 
    : IModel
{
    public IEnumerable<string> Configs { get; } = new List<string>();

    public void Add(string config) 
    {
        ((List<string>)Configs).Add(config);        
    }
}