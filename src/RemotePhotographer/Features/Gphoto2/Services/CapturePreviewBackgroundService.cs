
using System.Runtime.InteropServices;
using Boilerplate.Features.Reactive.Services;
using RemotePhotographer.Features.Gphoto2.Services.Interop;
using RemotePhotographer.Features.Photographer.Events;

namespace RemotePhotographer.Features.Gphoto2.Services
{
    public class CapturePreviewBackgroundService
        : BackgroundService
    {
        private object _door = new object();
        private CancellationToken _token;
        private bool _started;
        private int _fps;
        private readonly ICameraContextManager _manager;
        private readonly IMethodValidator _validator;
        private readonly IEventDispatcher _dispatcher;

        public CapturePreviewBackgroundService(
            ICameraContextManager manager,
            IMethodValidator validator, 
            IEventDispatcher dispatcher)
        {
            _manager = manager;
            _validator = validator;
            _dispatcher = dispatcher;
        }

        public Task StartPreviewAsync(int fps) 
        {
            lock(_door) 
            {
                _fps = fps;
            }

            if(_started) 
            {
                return Task.CompletedTask;
            }

            _started = true;
            _token = new CancellationToken();

            return base.StartAsync(_token);
        }

        public Task StopPreviewAsync() 
        {
            _started = false;
            return base.StopAsync(_token);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if(!_started) {
                return;
            }

            IntPtr cameraFilePathPointer = CreateCameraFilePathPointer();

            try 
            {
                while(true) 
                {
                    if(stoppingToken.IsCancellationRequested) 
                    {
                        return;
                    }

                    CapturePreviewImageWithCamera(cameraFilePathPointer);
                    var previewImageData = GetPreviewImageData(cameraFilePathPointer);
                    
                    _dispatcher.Dispatch(new PreviewImageCaptured(previewImageData));

                    await DelayAsync();
                }
            }
            finally 
            {
                FreeCameraFilePathPointer(cameraFilePathPointer);
            }
        }

        private Task DelayAsync()
        {
            int interval;

            lock(_door) 
            {
                if(_fps <= 0) {
                    throw new ArgumentOutOfRangeException("fps must be greater than 0");
                }

                interval = 1000 / _fps;
            }

            return Task.Delay(interval);
        }

        private IntPtr CreateCameraFilePathPointer()
        {
            _validator.Validate(
                FileService.gp_file_new(
                    out IntPtr cameraFilePath
                ), 
                nameof(FileService.gp_file_new)
            );

            return cameraFilePath;
        }

        private void FreeCameraFilePathPointer(IntPtr cameraFilePathPointer)
        {
            _validator.Validate(
                FileService.gp_file_free(cameraFilePathPointer), 
                nameof(FileService.gp_file_free)
            );
        }

        private byte[] GetPreviewImageData(IntPtr cameraFilePath)
        {
            _validator.Validate(
                FileService.gp_file_get_data_and_size(cameraFilePath, out IntPtr data, out ulong size),
                nameof(FileService.gp_file_get_data_and_size)
            );

            byte[] previewImageData = new byte[size];
            Marshal.Copy(data, previewImageData, 0, previewImageData.Length);

            return previewImageData;
        }

        private void CapturePreviewImageWithCamera(IntPtr cameraFilePath)
        {
            lock(_manager.Door) 
            {
                _validator.Validate(
                    CameraService.gp_camera_capture_preview(
                        _manager.CameraContext.Camera, 
                        cameraFilePath, 
                        _manager.CameraContext.Context
                    ), 
                    nameof(CameraService.gp_camera_capture_preview)
                );
            }
        }
    }
}
