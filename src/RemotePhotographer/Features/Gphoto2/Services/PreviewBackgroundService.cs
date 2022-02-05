using System.Runtime.InteropServices;
using Boilerplate.Features.Core.Commands;
using Boilerplate.Features.Reactive.Services;
using RemotePhotographer.Features.Gphoto2.Services.Interop;
using RemotePhotographer.Features.Photographer.Commands;
using RemotePhotographer.Features.Photographer.Events;

namespace RemotePhotographer.Features.Gphoto2.Services
{
    public class PreviewBackgroundService
        : BackgroundService, IPreviewService, IRecordingService
    {
        private object _door = new object();
        private CancellationTokenSource  _sessionCancellationTokenSource;
        private bool _started;
        private bool _recording;
        private int _fps;
        private readonly ICameraContextManager _manager;
        private readonly IMethodValidator _validator;
        private readonly IEventDispatcher _dispatcher;
        private readonly ICommandDispatcher _commandDispatcher;
        public PreviewBackgroundService(
            ICameraContextManager manager,
            IMethodValidator validator, 
            IEventDispatcher dispatcher,
            ICommandDispatcher commandDispatcher)
        {
            _manager = manager;
            _validator = validator;
            _dispatcher = dispatcher;
            _commandDispatcher = commandDispatcher;
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

            _sessionCancellationTokenSource = new();
            _started = true;

            return Task.CompletedTask;
        }

        public async Task StartRecodingAsync(int fps) 
        {
            await StartPreviewAsync(fps);

            lock(_door) 
            {
                _recording = true;
            }
        }

        public async Task StopRecodingAsync() 
        {   
            lock(_door) 
            {
                _recording = false;
            }

            await StopPreviewAsync();
        }

        public Task StopPreviewAsync() 
        {
            _sessionCancellationTokenSource.Cancel();
            return Task.CompletedTask;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(true) 
            {
                if(_started) 
                {
                     IntPtr cameraFilePathPointer = CreateCameraFilePathPointer();

                        try 
                        {
                            while(true) 
                            {
                                _sessionCancellationTokenSource.Token.ThrowIfCancellationRequested();
                                stoppingToken.ThrowIfCancellationRequested();

                                CapturePreviewImageWithCamera(cameraFilePathPointer);
                                var previewImageData = GetPreviewImageData(cameraFilePathPointer);
                                
                                lock(_door) 
                                {
                                    if(_recording) 
                                    {
                                        _dispatcher.Dispatch(new VideoImageCaptured(
                                            previewImageData, GetTags())
                                        );
                                    } 
                                    else 
                                    {
                                        _dispatcher.Dispatch(new PreviewImageCaptured(
                                            previewImageData, GetTags())
                                        );
                                    }
                                }
                                
                                await DelayAsync();
                            }
                        }
                        catch(Exception) 
                        {
                        }
                        finally
                        {
                            await CloseViewFinderAsync();
                            FreeCameraFilePathPointer(cameraFilePathPointer);
                            _started = false;
                        }
                }

                await Task.Delay(1000);
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

        private async Task CloseViewFinderAsync() 
        {
            await _commandDispatcher.DispatchAsync(new SetViewFinder(false));
        }

        private IEnumerable<string> GetTags() 
        {
            lock(_manager.Door) {

                _manager.EnsureCameraContext();

                var tags = new List<string>(_manager.CameraContext.Tags);
                tags.Add("preview");

                return tags;
            }        
        }
    }
}
