using System.Runtime.InteropServices;

namespace RemotePhotographer.Features.Gphoto2.Services.Interop;

public class ContextService 
{
    [DllImport("gphoto2")]
    public static extern IntPtr gp_context_new();

    [DllImport("gphoto2")]
    public static extern void gp_context_ref(IntPtr context);

    [DllImport("gphoto2")]
    public static extern void gp_context_unref(IntPtr context);
}