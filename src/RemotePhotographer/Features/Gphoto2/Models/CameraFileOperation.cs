
namespace RemotePhotographer.Features.Gphoto2.Models;

///http://www.gphoto.org/doc/api/gphoto2-abilities-list_8h.html#ad0d503f618c623e74b1c888c53d5a209
public enum CameraFileOperation
{
    GP_FILE_OPERATION_NONE = 0,
    GP_FILE_OPERATION_DELETE = 1 << 1,
    GP_FILE_OPERATION_PREVIEW = 1 << 3,
    GP_FILE_OPERATION_RAW = 1 << 4,
    GP_FILE_OPERATION_AUDIO = 1 << 5,
    GP_FILE_OPERATION_EXIF = 1 << 6
}