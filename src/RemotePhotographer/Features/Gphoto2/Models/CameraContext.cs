namespace RemotePhotographer.Features.Gphoto2.Models;

public class CameraContext
{
    public CameraContext(string cameraId, IntPtr context, IntPtr camera, IEnumerable<string> tags)
    {
        CameraId = cameraId;
        Context = context;
        Camera = camera;
        Tags = new List<string>(tags);
    }

    public string CameraId { get; }

    public IntPtr Context {get;}

    public IntPtr Camera {get;}

    public IEnumerable<string> Tags {get;}
}