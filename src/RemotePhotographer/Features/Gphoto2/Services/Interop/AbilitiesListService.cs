using System.Runtime.InteropServices;
using RemotePhotographer.Features.Gphoto2.Models;

namespace RemotePhotographer.Features.Gphoto2.Services.Interop;

public class AbilitiesListService 
{
    [DllImport("gphoto2")]
    public static extern int gp_abilities_list_new(out IntPtr cameraAbilitiesList);

    [DllImport("gphoto2")]
    public static extern int gp_abilities_list_free(IntPtr cameraAbilitiesList);

    [DllImport("gphoto2")]
    public static extern int gp_abilities_list_load(IntPtr cameraAbilitiesList, IntPtr context);

    [DllImport("gphoto2")]
    public static extern int gp_abilities_list_detect(IntPtr cameraAbilitiesList, IntPtr portInfoList, IntPtr cameraList, IntPtr context);

    [DllImport("gphoto2")]
    public static extern int gp_abilities_list_get_abilities(IntPtr cameraAbilitiesList, int index, out CameraAbilities cameraAbilities);

    [DllImport("gphoto2")]
    public static extern int gp_abilities_list_count(IntPtr list);

    [DllImport("gphoto2", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.Ansi)]
    public static extern int gp_abilities_list_lookup_model(IntPtr cameraAbilitiesList, [MarshalAs(UnmanagedType.LPStr)] string model);
}