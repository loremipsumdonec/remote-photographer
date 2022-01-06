using System.Runtime.InteropServices;

namespace RemotePhotographer.Features.Gphoto2.Services.Interop;

public class FileService 
{
    [DllImport("gphoto2")]
    public static extern int gp_file_get_data_and_size(IntPtr file, out IntPtr data, out ulong size);

    [DllImport("gphoto2")]
    public static extern int gp_file_new(out IntPtr file);

    [DllImport("gphoto2")]
    public static extern int gp_file_free(IntPtr file);
}