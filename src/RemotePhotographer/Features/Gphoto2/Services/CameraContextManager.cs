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

    public object Door { get; } = new object();

    public void EnsureCameraContext() 
    {
        if(CameraContext == null) 
        {
            throw new InvalidOperationException("No camera not connected");
        }        
    }

    public CameraContext CameraContext { get; set; }
}