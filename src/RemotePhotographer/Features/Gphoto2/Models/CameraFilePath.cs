using System.Runtime.InteropServices;

namespace RemotePhotographer.Features.Gphoto2.Models;

///http://www.gphoto.org/doc/api/structCameraFilePath.html
[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
public struct CameraFilePath
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst=128)]
    public sbyte[] name;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst=1024)]
    public sbyte[] folder;
}