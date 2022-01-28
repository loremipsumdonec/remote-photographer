namespace RemotePhotographer.Features.Gphoto2.Models;

public class CameraContext
{
    public CameraContext(IntPtr context, IntPtr camera, IEnumerable<string> tags)
    {
        Context = context;
        Camera = camera;
        Tags = new List<string>(tags);
    }

    public IntPtr Context {get;}

    public IntPtr Camera {get;}

    public IEnumerable<string> Tags {get;}
}