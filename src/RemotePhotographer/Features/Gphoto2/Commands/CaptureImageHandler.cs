using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using RemotePhotographer.Features.Gphoto2.Services.Interop;
using RemotePhotographer.Features.Gphoto2.Services;
using RemotePhotographer.Features.Photographer.Commands;

namespace RemotePhotographer.Features.Gphoto2.Commands;

[Handle(typeof(CaptureImage))]
public class CaptureImageHandler
    : CommandHandler<CaptureImage>
{
    private readonly ICameraContextManager _manager;

    public CaptureImageHandler(ICameraContextManager manager)
    {
        _manager = manager;
    }

    public override Task<bool> ExecuteAsync(CaptureImage command)
    {
        var status = CameraService.gp_camera_capture(
            _manager.CameraContext.Camera, 
            0, 
            out IntPtr path, 
            _manager.CameraContext.Context
        );

        return Task.FromResult(status == 0);
    }
}