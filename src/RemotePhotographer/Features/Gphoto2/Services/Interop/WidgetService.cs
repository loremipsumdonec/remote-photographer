using System.Runtime.InteropServices;
using RemotePhotographer.Features.Gphoto2.Models;

namespace RemotePhotographer.Features.Gphoto2.Services.Interop;

public class WidgetService 
{
        [DllImport("gphoto2")]
        public static extern int gp_widget_new(int type, [MarshalAs(UnmanagedType.LPStr)] string label, out IntPtr widget);

        [DllImport("gphoto2")]
        public static extern int gp_widget_free(IntPtr widget);

        [DllImport("gphoto2")]
        public static extern int gp_widget_ref(IntPtr widget);

        [DllImport("gphoto2")]
        public static extern int gp_widget_unref(IntPtr widget);

        [DllImport("gphoto2")]
        public static extern int gp_widget_changed(IntPtr widget);

        [DllImport("gphoto2")]
        public static extern int gp_widget_set_changed(IntPtr widget, int changed);

        [DllImport("gphoto2")]
        public static extern int gp_widget_get_type(IntPtr widget, out CameraWidgetType type);

        [DllImport("gphoto2")]
        public static extern int gp_widget_get_value(IntPtr widget, out IntPtr value);

        [DllImport("gphoto2")]
        public static extern int gp_widget_set_value(IntPtr widget, IntPtr value);

        [DllImport("gphoto2")]
        public static extern int gp_widget_get_label(IntPtr widget, out IntPtr label);

        [DllImport("gphoto2")]
        public static extern int gp_widget_get_name(IntPtr widget, out IntPtr name);

        [DllImport("gphoto2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int gp_widget_set_name(IntPtr widget, [MarshalAs(UnmanagedType.LPStr)] string name);

        [DllImport("gphoto2")]
        public static extern int gp_widget_get_info(IntPtr widget, out IntPtr info);

        [DllImport("gphoto2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int gp_widget_set_info(IntPtr widget, [MarshalAs(UnmanagedType.LPStr)] string info);

        [DllImport("gphoto2")]
        public static extern int gp_widget_get_id(IntPtr widget, out int id);

        [DllImport("gphoto2")]
        public static extern int gp_widget_get_readonly(IntPtr widget, out int ro);

        [DllImport("gphoto2")]
        public static extern int gp_widget_set_readonly(IntPtr widget, int ro);

        [DllImport("gphoto2")]
        public static extern int gp_widget_get_parent(IntPtr widget, out IntPtr parent);

        [DllImport("gphoto2")]
        public static extern int gp_widget_get_root(IntPtr widget, out IntPtr root);

        [DllImport("gphoto2")]
        public static extern int gp_widget_add_choice(IntPtr widget, [MarshalAs(UnmanagedType.LPStr)] string choice);

        [DllImport("gphoto2")]
        public static extern int gp_widget_count_choices(IntPtr widget);

        [DllImport("gphoto2")]
        public static extern int gp_widget_get_choice(IntPtr widget, int choice_number, out IntPtr choice);
}