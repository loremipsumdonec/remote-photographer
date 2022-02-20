using Boilerplate.Features.Core.Commands;
using Boilerplate.Features.Reactive.Services;
using RemotePhotographer.Features.Auto.Models;
using RemotePhotographer.Features.Photographer.Commands;
using RemotePhotographer.Features.Auto.Actions;
using RemotePhotographer.Features.Auto.Events;

namespace RemotePhotographer.Features.Auto.Services
{
    public class SessionBackgroundService
        : BackgroundService
    {
        private CancellationTokenSource  _sessionCancellationTokenSource;

        private bool _started;

        private Session _session;

        private readonly ICommandDispatcher _commandDispatcher;
        
        private readonly IEventDispatcher _eventDispatcher;

        private string _cameraId;

        public SessionBackgroundService(
            ICommandDispatcher commandDispatcher,
            IEventDispatcher eventDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _eventDispatcher = eventDispatcher;
            
        }

        public Task StartSessionAsync(string cameraId, Session session) 
        {
            if(_started) 
            {
                return Task.CompletedTask;
            }

            _sessionCancellationTokenSource = new();
            _session = session;
            _cameraId = cameraId;
            _started = true;

            return Task.CompletedTask;
        }

        public Task StopSessionAsync() 
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
                    bool stopped = false, failed = false;

                    try 
                    {
                        await RunSessionAsync(_sessionCancellationTokenSource.Token, stoppingToken);
                    }
                    catch(OperationCanceledException) 
                    {
                        stopped = true;
                        _eventDispatcher.Dispatch(new SessionStopped());

                        if(stoppingToken.IsCancellationRequested) 
                        {
                            break;
                        }
                    }
                    catch(Exception) 
                    {
                        failed = true;
                        _eventDispatcher.Dispatch(new SessionFailed());
                    }
                    finally 
                    {
                        if(!failed && !stopped) 
                        {
                            _eventDispatcher.Dispatch(new SessionFinished());
                        }

                        _started = false;
                    }
                }

                await Task.Delay(1000);
            }
        }

        private async Task RunSessionAsync(CancellationToken sessionStoppingToken, CancellationToken serviceStoppingToken)
        {
            foreach(var action in _session.Actions)         
            {
                sessionStoppingToken.ThrowIfCancellationRequested();
                serviceStoppingToken.ThrowIfCancellationRequested();

                _eventDispatcher.Dispatch(new ActionStarted());

                if(action is CaptureImageAction captureImageAction) 
                {
                    await CaptureImageActionAsync(
                        captureImageAction, 
                        sessionStoppingToken,
                        serviceStoppingToken
                    );
                }

                _eventDispatcher.Dispatch(new ActionFinished());     
            }
        }

        private async Task CaptureImageActionAsync(
            CaptureImageAction action, 
            CancellationToken sessionStoppingToken, 
            CancellationToken serviceStoppingToken
        )
        {
            if(!string.IsNullOrEmpty(action.ShutterSpeed)) 
            {
                await _commandDispatcher.DispatchAsync(new SetShutterSpeed(_cameraId, action.ShutterSpeed));
            }

            if(!string.IsNullOrEmpty(action.ISO)) 
            {
                await _commandDispatcher.DispatchAsync(new SetISO(_cameraId, action.ISO));
            }

            if(!string.IsNullOrEmpty(action.Aperture)) 
            {
                await _commandDispatcher.DispatchAsync(new SetAperture(_cameraId, action.Aperture));
            }

            if(!string.IsNullOrEmpty(action.ImageFormat)) 
            {
                await _commandDispatcher.DispatchAsync(new SetImageFormat(_cameraId, action.ImageFormat));
            }

            for(int index = 0; index < action.Exposures; index++) 
            {
                sessionStoppingToken.ThrowIfCancellationRequested();
                serviceStoppingToken.ThrowIfCancellationRequested();

                await _commandDispatcher.DispatchAsync(new CaptureImage(_cameraId));
            }
        }
    }
}
