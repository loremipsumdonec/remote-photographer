using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using RemotePhotographer.Features.Gphoto2.Services;
using RemotePhotographer.Features.Photographer.Commands;

namespace RemotePhotographer.Features.Gphoto2.Commands;

[Handle(typeof(ConnectCamera))]
public class ConnectCameraHandler
    : CommandHandler<ConnectCamera>
{
    private readonly ICameraContextManager _manager;

    public ConnectCameraHandler(ICameraContextManager manager)
    {
        _manager = manager;
    }

    public override Task<bool> ExecuteAsync(ConnectCamera query)
    {
        _manager.ConnectCamera();

        return Task.FromResult(true);
    }
}