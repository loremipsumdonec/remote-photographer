using System.Runtime.InteropServices;
using RemotePhotographer.Features.Gphoto2.Models;

namespace RemotePhotographer.Features.Gphoto2.Services.Interop;

public class PortInfoListService 
{
    [DllImport("gphoto2")]
    public static extern int gp_port_info_list_new(out IntPtr portInfoList);

    [DllImport("gphoto2")]
    public static extern int gp_port_info_list_free(IntPtr portInfoList);

    [DllImport("gphoto2")]
    public static extern int gp_port_info_list_count(IntPtr portInfoList);

    [DllImport("gphoto2")]
    public static extern int gp_port_info_list_load(IntPtr portInfoList);

    [DllImport("gphoto2")]
    public static extern int gp_port_info_list_get_info(IntPtr portInfoList, int n, out IntPtr info);

    [DllImport("gphoto2")]
    public static extern int gp_port_info_get_name(IntPtr portInfoList, out IntPtr name);

    [DllImport("gphoto2")]
    public static extern int gp_port_info_get_path(IntPtr portInfoList, out IntPtr name);

    [DllImport("gphoto2")]
    public static extern int gp_port_info_get_type(IntPtr portInfoList, out GPPortType type);

    [DllImport("gphoto2")]
    public static extern int gp_port_info_list_lookup_path(IntPtr portInfoList, [MarshalAs(UnmanagedType.LPStr)] string path);

}