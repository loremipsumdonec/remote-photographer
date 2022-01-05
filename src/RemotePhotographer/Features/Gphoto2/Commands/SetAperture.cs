using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using RemotePhotographer.Features.Gphoto2.Services.Interop;
using RemotePhotographer.Features.Gphoto2.Services;
using RemotePhotographer.Features.Photographer.Commands;

namespace RemotePhotographer.Features.Gphoto2.Commands;

[Handle(typeof(SetAperture))]
public class SetApertureHandler
    : CommandHandler<SetAperture>
{
    private readonly ICameraContextManager _manager;

    public SetApertureHandler(ICameraContextManager manager)
    {
        _manager = manager;
    }

    public override Task<bool> ExecuteAsync(SetAperture command)
    {
        return Task.FromResult(false);
    }
}