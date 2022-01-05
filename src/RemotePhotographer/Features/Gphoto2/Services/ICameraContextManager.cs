using RemotePhotographer.Features.Gphoto2.Models;

namespace RemotePhotographer.Features.Gphoto2.Services;

public interface ICameraContextManager
{
    CameraContext CameraContext {get;}

    void ConnectCamera();

    void DisconnectCamera();
}