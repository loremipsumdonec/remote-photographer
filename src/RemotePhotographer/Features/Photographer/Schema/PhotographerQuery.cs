using Boilerplate.Features.Core.Queries;
using RemotePhotographer.Features.Photographer.Models;
using RemotePhotographer.Features.Photographer.Queries;

namespace RemotePhotographer.Features.Photographer.Schema;

public class PhotographerQuery
{
    public Task<GetPortsModel> Ports([Service] IQueryDispatcher dispatcher) 
    {
        return dispatcher.DispatchAsync<GetPortsModel>(
            new GetPorts()
        );
    }

    public Task<GetCamerasModel> Cameras([Service] IQueryDispatcher dispatcher) 
    {
        return dispatcher.DispatchAsync<GetCamerasModel>(
            new GetCameras()
        );
    }

    public Task<Camera> Camera([Service] IQueryDispatcher dispatcher) 
    {
        return dispatcher.DispatchAsync<Camera>(
            new GetCamera()
        );
    }

    public Task<GetFilesModel> Files(string path, [Service] IQueryDispatcher dispatcher) 
    {
        return dispatcher.DispatchAsync<GetFilesModel>(
            new GetFiles(path)
        );
    }

    public Task<GetFoldersModel> Folders(string path, [Service] IQueryDispatcher dispatcher) 
    {
        return dispatcher.DispatchAsync<GetFoldersModel>(
            new GetFolders(path)
        );
    }

    public Task<ISO> Iso([Service] IQueryDispatcher dispatcher)
    {
        return dispatcher.DispatchAsync<ISO>(
            new GetISO()
        );
    }

    public Task<ShutterSpeed> ShutterSpeed([Service] IQueryDispatcher dispatcher)
    {
        return dispatcher.DispatchAsync<ShutterSpeed>(
            new GetShutterSpeed()
        );
    }
}