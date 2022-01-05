using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using RemotePhotographer.Features.Gphoto2.Services.Interop;
using RemotePhotographer.Features.Gphoto2.Services;
using RemotePhotographer.Features.Photographer.Commands;

namespace RemotePhotographer.Features.Gphoto2.Commands;

[Handle(typeof(SetISO))]
public class SetISOHandler
    : CommandHandler<SetISO>
{
    private readonly ICameraContextManager _manager;

    public SetISOHandler(ICameraContextManager manager)
    {
        _manager = manager;
    }

    public override Task<bool> ExecuteAsync(SetISO command)
    {
        return Task.FromResult(false);
    }
}