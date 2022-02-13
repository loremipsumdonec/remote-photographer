
namespace RemotePhotographer.Features.Gphoto2.Services;

public interface IPreviewService
{
    Task StartPreviewAsync(int fps);

    Task StopPreviewAsync();
}