using RemotePhotographer.Features.Gphoto2.Models;

namespace RemotePhotographer.Features.Gphoto2.Services;

public interface ICameraContextManager
{
    object Door { get; }

    void EnsureCameraContext();

    CameraContext CameraContext { get; set; }
}