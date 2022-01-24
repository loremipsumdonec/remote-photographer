using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using RemotePhotographer.Features.Gphoto2.Services.Interop;
using RemotePhotographer.Features.Gphoto2.Services;
using RemotePhotographer.Features.Photographer.Commands;
using Boilerplate.Features.Reactive.Services;
using Boilerplate.Features.Core.Queries;
using System.Runtime.InteropServices;

namespace RemotePhotographer.Features.Gphoto2.Commands;

[Handle(typeof(CapturePreviewImage))]
public class CapturePreviewImageHandler
    : CommandHandler<CapturePreviewImage>
{
    private readonly ICameraContextManager _manager;
    private readonly IEventDispatcher _dispatcher;
    private readonly IMethodValidator _validator;
    private readonly IQueryDispatcher _queryDispatcher;

    public CapturePreviewImageHandler(
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

    public override Task<bool> ExecuteAsync(CapturePreviewImage command)
    {
        _validator.Validate(
            FileService.gp_file_new(
                out IntPtr cameraFilePath
            ), 
            nameof(FileService.gp_file_new)
        );
        
        _validator.Validate(
            CameraService.gp_camera_capture_preview(
                _manager.CameraContext.Camera, 
                cameraFilePath, 
                _manager.CameraContext.Context
            ), 
            nameof(CameraService.gp_camera_capture_preview)
        );

        _validator.Validate(
            FileService.gp_file_get_data_and_size(cameraFilePath, out IntPtr data, out ulong size),
            nameof(FileService.gp_file_get_data_and_size)
        );

        byte[] da = new byte[size];
        Marshal.Copy(data, da, 0, da.Length);

        _validator.Validate(
            FileService.gp_file_free(cameraFilePath), 
            nameof(FileService.gp_file_free)
        );

        return Task.FromResult(true);
    }
}