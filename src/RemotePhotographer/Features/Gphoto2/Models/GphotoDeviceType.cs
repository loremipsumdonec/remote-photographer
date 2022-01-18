
namespace RemotePhotographer.Features.Gphoto2.Models;

///http://www.gphoto.org/doc/api/gphoto2-abilities-list_8h.html#af5705e04f9b4996bdb37675fcde7126e
public enum GphotoDeviceType
{
    GP_DEVICE_STILL_CAMERA = 0,
    GP_DEVICE_AUDIO_PLAYER = 1 << 0
}