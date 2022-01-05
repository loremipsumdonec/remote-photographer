using System.Runtime.InteropServices;

namespace RemotePhotographer.Features.Gphoto2;

public enum VersionVerbosity 
{
    Short = 0,
    Verbose = 1
}

public static class VersionInterop 
{
    [DllImport("gphoto2", CharSet = CharSet.Ansi)]
    public static extern IntPtr gp_library_version(VersionVerbosity verbose);
}