using System.Runtime.InteropServices;

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
    public static extern int gp_abilities_list_get_abilities(IntPtr cameraAbilitiesList, int index, out IntPtr cameraAbilities);
}