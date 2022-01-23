using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;
using RemotePhotographer.Features.Photographer.Models;

namespace RemotePhotographer.Features.Photographer.Queries;

public class GetPorts 
    : Query
{
}

public class GetPortsModel 
    : IModel
{
    public IEnumerable<PortInfo> Ports { get; } = new List<PortInfo>();

    public void Add(PortInfo portInfo) 
    {
        if(portInfo == null) 
        {
            return;
        }

        ((List<PortInfo>)Ports).Add(portInfo);        
    }
}