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

    public Task<GetConfigsModel> Configs(string cameraId, [Service] IQueryDispatcher dispatcher) 
    {
        return dispatcher.DispatchAsync<GetConfigsModel>(
            new GetConfigs(cameraId)
        );
    }

    public Task<GetCamerasModel> Cameras([Service] IQueryDispatcher dispatcher) 
    {
        return dispatcher.DispatchAsync<GetCamerasModel>(
            new GetCameras()
        );
    }

    public Task<GetFilesModel> Files(string cameraId, string path, [Service] IQueryDispatcher dispatcher) 
    {
        return dispatcher.DispatchAsync<GetFilesModel>(
            new GetFiles(cameraId, path)
        );
    }

    public Task<GetFoldersModel> Folders(string cameraId, string path, [Service] IQueryDispatcher dispatcher) 
    {
        return dispatcher.DispatchAsync<GetFoldersModel>(
            new GetFolders(cameraId, path)
        );
    }

    public Task<BatteryLevel> BatteryLevel(string cameraId, [Service] IQueryDispatcher dispatcher)
    {
        return dispatcher.DispatchAsync<BatteryLevel>(
            new GetBatteryLevel(cameraId)
        );
    }

    public Task<Aperture> Aperture(string cameraId, [Service] IQueryDispatcher dispatcher)
    {
        return dispatcher.DispatchAsync<Aperture>(
            new GetAperture(cameraId)
        );
    }

    public Task<ISO> Iso(string cameraId, [Service] IQueryDispatcher dispatcher)
    {
        return dispatcher.DispatchAsync<ISO>(
            new GetISO(cameraId)
        );
    }

    public Task<ShutterSpeed> ShutterSpeed(string cameraId, [Service] IQueryDispatcher dispatcher)
    {
        return dispatcher.DispatchAsync<ShutterSpeed>(
            new GetShutterSpeed(cameraId)
        );
    }

    public Task<CaptureTarget> CaptureTarget(string cameraId, [Service] IQueryDispatcher dispatcher)
    {
        return dispatcher.DispatchAsync<CaptureTarget>(
            new GetCaptureTarget(cameraId)
        );
    }

        public Task<ImageFormat> ImageFormat(string cameraId, [Service] IQueryDispatcher dispatcher)
    {
        return dispatcher.DispatchAsync<ImageFormat>(
            new GetImageFormat(cameraId)
        );
    }
}