using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using RemotePhotographer.Features.Gphoto2.Services.Interop;
using RemotePhotographer.Features.Gphoto2.Services;
using RemotePhotographer.Features.Photographer.Commands;
using Boilerplate.Features.Reactive.Services;
using RemotePhotographer.Features.Photographer.Events;
using RemotePhotographer.Features.Gphoto2.Models;
using RemotePhotographer.Features.Gphoto2.Extensions;

namespace RemotePhotographer.Features.Gphoto2.Commands;

[Handle(typeof(CaptureImage))]
public class CaptureImageHandler
    : CommandHandler<CaptureImage>
{
    private readonly ICameraContextManager _manager;
    private readonly IEventDispatcher _dispatcher;

    public CaptureImageHandler(ICameraContextManager manager, IEventDispatcher dispatcher)
    {
        _manager = manager;
        _dispatcher = dispatcher;
    }

    public override Task<bool> ExecuteAsync(CaptureImage command)
    {
        var status = CameraService.gp_camera_capture(
            _manager.CameraContext.Camera, 
            0, 
            out CameraFilePath cameraFilePath, 
            _manager.CameraContext.Context
        );

        if(status == 0) 
        {
            string folder = cameraFilePath.folder.ConvertToString();
            string name = cameraFilePath.name.ConvertToString();

            _dispatcher.Dispatch(
                new ImageCaptured(System.IO.Path.Combine(folder, name))
            );
        }

        return Task.FromResult(status == 0);
    }
}