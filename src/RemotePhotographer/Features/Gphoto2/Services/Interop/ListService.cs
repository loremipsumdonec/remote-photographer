using System.Runtime.InteropServices;

namespace RemotePhotographer.Features.Gphoto2.Services.Interop;

public class ListService 
{
        [DllImport("gphoto2")]
        public static extern int gp_list_new(out IntPtr list);

        [DllImport("gphoto2")]
        public static extern int gp_list_free(IntPtr list);

        [DllImport("gphoto2")]
        public static extern int gp_list_count(IntPtr list);

        [DllImport("gphoto2")]
        public static extern int gp_list_get_name(IntPtr list, int index, out IntPtr name);

        [DllImport("gphoto2")]
        public static extern int gp_list_get_value(IntPtr list, int index, out IntPtr value);
}