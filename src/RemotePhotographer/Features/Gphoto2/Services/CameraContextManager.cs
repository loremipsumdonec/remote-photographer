using RemotePhotographer.Features.Gphoto2.Services.Interop;
using RemotePhotographer.Features.Gphoto2.Models;

namespace RemotePhotographer.Features.Gphoto2.Services;

public class CameraContextManager
    : ICameraContextManager
{
    private IMethodValidator _validator;

    public CameraContextManager(IMethodValidator validator)
    {
        _validator = validator;
    }

    public CameraContext CameraContext {get; set;}
}