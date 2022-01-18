using System.Runtime.InteropServices;

namespace RemotePhotographer.Features.Gphoto2.Models;

///http://www.gphoto.org/doc/api/structCameraText.html
[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
public struct CameraText
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst=32 * 1024)]
    public sbyte[] text;
}