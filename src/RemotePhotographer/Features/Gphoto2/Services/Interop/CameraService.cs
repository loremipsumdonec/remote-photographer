using System.Runtime.InteropServices;
using RemotePhotographer.Features.Gphoto2.Models;

namespace RemotePhotographer.Features.Gphoto2.Services.Interop;

public class CameraService 
{
    [DllImport("gphoto2")]
    public static extern int gp_camera_new(out IntPtr camera);

    [DllImport("gphoto2")]
    public static extern int gp_camera_free(IntPtr camera);

    [DllImport("gphoto2")]
    public static extern int gp_camera_init(IntPtr camera, IntPtr context);

    [DllImport("gphoto2")]
    public static extern int gp_camera_exit(IntPtr camera, IntPtr context);

    [DllImport("gphoto2")]
    public static extern int gp_camera_capture_preview(IntPtr camera, IntPtr file, IntPtr context);

    [DllImport("gphoto2")]
    public static extern int gp_camera_capture(IntPtr camera, int type, out CameraFilePath path, IntPtr context);

    [DllImport("gphoto2")]
    public static extern int gp_camera_wait_for_event(IntPtr camera, int timeout, out IntPtr eventtype, out IntPtr eventdata, IntPtr context);

    [DllImport("gphoto2")]
    public static extern int gp_camera_file_get(IntPtr camera,sbyte[] folder, sbyte[] filename, short type, IntPtr camera_file, IntPtr context);

    [DllImport("gphoto2")]
    public static extern int gp_camera_file_delete(IntPtr camera, sbyte[] folder, sbyte[] filename, IntPtr context);

    [DllImport("gphoto2")]
    public static extern int gp_camera_folder_list_files(IntPtr camera, sbyte[] folder, IntPtr list, IntPtr context);

    [DllImport("gphoto2")]
    public static extern int gp_camera_folder_list_folders(IntPtr camera, sbyte[] folder, IntPtr list, IntPtr context);

    [DllImport("gphoto2")]
    public static extern int gp_camera_autodetect(IntPtr cameraList, IntPtr context);

    [DllImport("gphoto2")]
    public static extern int gp_camera_get_port_info(IntPtr camera, out IntPtr info);

    [DllImport("gphoto2")]
    public static extern int gp_camera_set_port_info(IntPtr camera, IntPtr info);

    [DllImport("gphoto2", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.Ansi)]
    public static extern int gp_camera_get_single_config(IntPtr camera, [MarshalAs(UnmanagedType.LPStr)] string name, out IntPtr cameraWidget, IntPtr context);

    [DllImport("gphoto2")]
    public static extern int gp_camera_list_config(IntPtr camera, IntPtr cameraList, IntPtr context);
    
    [DllImport("gphoto2", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.Ansi)]
    public static extern int gp_camera_set_single_config(IntPtr camera, [MarshalAs(UnmanagedType.LPStr)] string name, IntPtr widget, IntPtr context);

    [DllImport("gphoto2")]
    public static extern int gp_camera_get_config(IntPtr camera, out IntPtr window, IntPtr context);

    [DllImport("gphoto2")]
    public static extern int gp_camera_get_about(IntPtr camera, out CameraText about, IntPtr context);

    [DllImport("gphoto2")]
    public static extern int gp_camera_get_summary(IntPtr camera, out CameraText summary, IntPtr context);

    [DllImport("gphoto2")]
    public static extern int gp_camera_get_abilities(IntPtr camera, out CameraAbilities abilities);

    [DllImport("gphoto2")]
    public static extern int gp_camera_set_abilities(IntPtr camera, CameraAbilities abilities);
}