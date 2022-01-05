using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;
using RemotePhotographer.Features.Photographer.Models;

namespace RemotePhotographer.Features.Photographer.Queries;

public class GetCameras 
    : Query
{
}

public class GetCamerasModel 
    : IModel
{
    public IEnumerable<Camera> Cameras { get; } = new List<Camera>();

    public void Add(Camera camera) 
    {
        if(camera == null) 
        {
            return;
        }

        ((List<Camera>)Cameras).Add(camera);        
    }
}