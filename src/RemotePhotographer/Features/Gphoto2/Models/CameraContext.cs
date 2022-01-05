namespace RemotePhotographer.Features.Gphoto2.Models;

public class CameraContext
{
    public CameraContext(IntPtr context, IntPtr camera)
    {
        Context = context;
        Camera = camera;
    }

    public IntPtr Context {get;}

    public IntPtr Camera {get;}
}