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
        
        Validator.Validate(
            CameraService.gp_camera_new(out IntPtr camera), 
            nameof(CameraService.gp_camera_new)
        );

        Validator.Validate(
            CameraService.gp_camera_init(camera, context), 
            nameof(CameraService.gp_camera_init)
        );

        CameraContext = new CameraContext(context, camera);
    }

    public void DisconnectCamera() 
    {
        var cameraContext = CameraContext;
        CameraContext = null;

        Validator.Validate(
            CameraService.gp_camera_exit(cameraContext.Camera, cameraContext.Context),
            nameof(CameraService.gp_camera_exit)
        );

        Validator.Validate(
            CameraService.gp_camera_free(cameraContext.Camera),
            nameof(CameraService.gp_camera_free)
        );

        ContextService.gp_context_unref(cameraContext.Context);
    }
}