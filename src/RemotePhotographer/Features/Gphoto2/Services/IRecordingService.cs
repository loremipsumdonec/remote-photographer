using RemotePhotographer.Features.Gphoto2.Models;

namespace RemotePhotographer.Features.Gphoto2.Services;

public interface IRecordingService
{
    Task StartRecodingAsync(int fps);

    Task StopRecodingAsync();
}