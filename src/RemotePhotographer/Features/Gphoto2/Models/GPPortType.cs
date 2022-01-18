
namespace RemotePhotographer.Features.Gphoto2.Models;

///http://www.gphoto.org/doc/api/gphoto2-port-info-list_8h.html#a0607050fab17d2a8c0cbff5747aadc06
public enum GPPortType
{
    GP_PORT_NONE = 0,
    GP_PORT_SERIAL = 1 << 0,
    GP_PORT_USB = 1 << 2,
    GP_PORT_DISK = 1 << 3,
    GP_PORT_PTPIP = 1 << 4,
    GP_PORT_USB_DISK_DIRECT = 1 << 5,
    GP_PORT_USB_SCSI  = 1 << 6
}