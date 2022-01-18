
namespace RemotePhotographer.Features.Gphoto2.Models;

///http://www.gphoto.org/doc/api/gphoto2-abilities-list_8h.html#ad0d503f618c623e74b1c888c53d5a209
public enum CameraOperation
{
    GP_OPERATION_NONE  = 0,
    GP_OPERATION_CAPTURE_IMAGE  = 1 << 0, 
    GP_OPERATION_CAPTURE_VIDEO = 1 << 1,
    GP_OPERATION_CAPTURE_AUDIO = 1 << 2,
    GP_OPERATION_CAPTURE_PREVIEW = 1 << 3,
    GP_OPERATION_CONFIG = 1 << 4,
    GP_OPERATION_TRIGGER_CAPTURE = 1 << 5 
}