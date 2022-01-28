using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using RemotePhotographer.Features.Gphoto2.Services.Interop;
using RemotePhotographer.Features.Gphoto2.Services;
using RemotePhotographer.Features.Photographer.Commands;
using Boilerplate.Features.Reactive.Services;
using RemotePhotographer.Features.Photographer.Events;
using RemotePhotographer.Features.Gphoto2.Models;
using RemotePhotographer.Features.Gphoto2.Extensions;
using Boilerplate.Features.Core.Queries;
using RemotePhotographer.Features.Photographer.Queries;

namespace RemotePhotographer.Features.Gphoto2.Commands;

[Handle(typeof(CaptureImage))]
public class CaptureImageHandler
    : CommandHandler<CaptureImage>
{
    private readonly ICameraContextManager _manager;
    private readonly IEventDispatcher _dispatcher;
    private readonly IMethodValidator _validator;
    private readonly IQueryDispatcher _queryDispatcher;

    public CaptureImageHandler(
        ICameraContextManager manager,
        IEventDispatcher dispatcher,
        IMethodValidator validator, 
        IQueryDispatcher queryDispatcher)
    {
        _manager = manager;
        _dispatcher = dispatcher;
        _validator = validator;
        _queryDispatcher = queryDispatcher;
    }

    public override async Task<bool> ExecuteAsync(CaptureImage command)
    {
        string imageFile = CaptureImageWithCamera();
        var image = await CreateImageFromFileAsync(imageFile);

        _dispatcher.Dispatch(new ImageCaptured(imageFile, image.Data, GetTags()));

        return true;
    }

    private IEnumerable<string> GetTags() 
    {
        lock(_manager.Door) {

            _manager.EnsureCameraContext();

            return _manager.CameraContext.Tags;
        }        
    }

    private string CaptureImageWithCamera() 
    {
        lock(_manager.Door) 
        {
            _manager.EnsureCameraContext();

            _validator.Validate(
                CameraService.gp_camera_capture(
                    _manager.CameraContext.Camera, 
                    0, 
                    out CameraFilePath cameraFilePath, 
                    _manager.CameraContext.Context
                ), 
                nameof(CameraService.gp_camera_capture)
            );

            string path = System.IO.Path.Combine(
                cameraFilePath.folder.ConvertToString(),
                cameraFilePath.name.ConvertToString()
            );

            return path;
        }
    }
    
    private async Task<GetImageModel> CreateImageFromFileAsync(string imageFile) 
    {
        return await _queryDispatcher.DispatchAsync<GetImageModel>(new GetImage(imageFile));
    }
}