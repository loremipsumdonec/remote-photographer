
namespace RemotePhotographer.Features.Gphoto2.Models;

///http://www.gphoto.org/doc/api/gphoto2-abilities-list_8h.html#a91d708ad7663e0bd7eaa8ba67b011d3c
public enum CameraFolderOperation
{
    GP_FOLDER_OPERATION_NONE = 0,
    GP_FOLDER_OPERATION_DELETE_ALL = 1 << 0,
    GP_FOLDER_OPERATION_PUT_FILE = 1 << 1,
    GP_FOLDER_OPERATION_MAKE_DIR = 1 << 2,
    GP_FOLDER_OPERATION_REMOVE_DIR = 1 << 3
}