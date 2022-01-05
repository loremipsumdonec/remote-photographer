using RemotePhotographer.Features.Gphoto2;
using System.Runtime.InteropServices;

namespace RemotePhotographer.Features.Photographer.Services;

public class CameraServiceOher
    : ICameraService
{
    public IEnumerable<object> GetConfigs() 
    {
        var obj = VersionInterop.gp_library_version(VersionVerbosity.Verbose);
        string portName = Marshal.PtrToStringUTF8(obj);

        return new List<object>();
    }
}