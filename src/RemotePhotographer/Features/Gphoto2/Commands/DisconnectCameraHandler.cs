using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using RemotePhotographer.Features.Gphoto2.Services;
using RemotePhotographer.Features.Photographer.Commands;

namespace RemotePhotographer.Features.Gphoto2.Commands;

[Handle(typeof(DisconnectCamera))]
public class DisconnectCameraHandler
    : CommandHandler<DisconnectCamera>
{
    private readonly ICameraContextManager _manager;

    public DisconnectCameraHandler(ICameraContextManager manager)
    {
        _manager = manager;
    }

    public override Task<bool> ExecuteAsync(DisconnectCamera query)
    {
        _manager.DisconnectCamera();

        return Task.FromResult(true);
    }
}