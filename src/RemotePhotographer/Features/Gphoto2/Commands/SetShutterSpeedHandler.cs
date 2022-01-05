using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using RemotePhotographer.Features.Gphoto2.Services.Interop;
using RemotePhotographer.Features.Gphoto2.Services;
using RemotePhotographer.Features.Photographer.Commands;

namespace RemotePhotographer.Features.Gphoto2.Commands;

[Handle(typeof(SetShutterSpeed))]
public class SetShutterSpeedHandler
    : CommandHandler<SetShutterSpeed>
{
    private readonly ICameraContextManager _manager;

    public SetShutterSpeedHandler(ICameraContextManager manager)
    {
        _manager = manager;
    }

    public override Task<bool> ExecuteAsync(SetShutterSpeed command)
    {
        return Task.FromResult(false);
    }
}