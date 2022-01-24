
using System.Runtime.InteropServices;
using RemotePhotographer.Features.Gphoto2.Services.Interop;

namespace RemotePhotographer.Features.Gphoto2.Services
{
    public class CapturePreviewBackgroundService
        : BackgroundService
    {
        private CancellationToken _token;
        private bool _started;
        private int _fps;
        private readonly ICameraContextManager _manager;
        private readonly IMethodValidator _validator;

        public CapturePreviewBackgroundService(
            ICameraContextManager manager, 
            IMethodValidator validator)
        {
            _manager = manager;
            _validator = validator;
        }

        public Task StartPreviewAsync(int fps) 
        {
            _started = true;
            _fps = fps;

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

            if(_fps <= 0) {
                throw new ArgumentOutOfRangeException("fps must be greater than 0");
            }

            if(_manager.CameraContext == null)
            {
                throw new InvalidOperationException("No camera connected");
            }

            int interval = 1000 / _fps;

            _validator.Validate(
                FileService.gp_file_new(
                    out IntPtr cameraFilePath
                ), 
                nameof(FileService.gp_file_new)
            );

            while(!stoppingToken.IsCancellationRequested) 
            {
                lock(_manager.CameraContext) 
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
                
                _validator.Validate(
                    FileService.gp_file_get_data_and_size(cameraFilePath, out IntPtr data, out ulong size),
                    nameof(FileService.gp_file_get_data_and_size)
                );

                byte[] da = new byte[size];
                Marshal.Copy(data, da, 0, da.Length);
                
                Console.WriteLine(da.Length);
                await Task.Delay(interval);
            }

            _validator.Validate(
                FileService.gp_file_free(cameraFilePath), 
                nameof(FileService.gp_file_free)
            );
        }
    }
}
