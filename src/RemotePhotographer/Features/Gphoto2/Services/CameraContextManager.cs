using RemotePhotographer.Features.Gphoto2.Services.Interop;
using RemotePhotographer.Features.Gphoto2.Models;

namespace RemotePhotographer.Features.Gphoto2.Services;

public class CameraContextManager
    : ICameraContextManager
{
    public CameraContext CameraContext {get; private set;}

    public void ConnectCamera() 
    {
        var context = ContextService.gp_context_new();
        var cameraNewStatus = CameraService.gp_camera_new(out IntPtr camera);
        var cameraInitStatus = CameraService.gp_camera_init(camera, context);

        CameraContext = new CameraContext(context, camera);
    }

    public void DisconnectCamera() 
    {
        var cameraContext = CameraContext;
        CameraContext = null;

        CameraService.gp_camera_exit(cameraContext.Camera, cameraContext.Context);
        CameraService.gp_camera_free(cameraContext.Camera);
        ContextService.gp_context_unref(cameraContext.Context);
    }
}