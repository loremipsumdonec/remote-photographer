using System.Runtime.InteropServices;

namespace RemotePhotographer.Features.Gphoto2.Models;

///http://www.gphoto.org/doc/api/structCameraAbilities.html
[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
public struct CameraAbilities
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst=128)]
    public sbyte[] model;

    public CameraDriverStatus status;

    public GPPortType port;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst=64)]
    public int[] speed;

    public CameraOperation operations;
    
    public CameraFileOperation file_operations;

    public CameraFolderOperation folder_operations;

    public int usb_vendor;

    public int usb_product;

    public int usb_class;

    public int usb_subclass;

    public int usb_protocol;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst=1024)]
    public sbyte[] library;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst=1024)]
    public sbyte[] id;

    public GphotoDeviceType device_type;

    public int reserved2;
    
    public int reserved3;
    
    public int reserved4;
    
    public int reserved5;
    
    public int reserved6;
    
    public int reserved7;
    
    public int reserved8;
}