namespace RemotePhotographer.Features.Photographer.Services;

public interface ICameraService 
{
    public IEnumerable<object> GetConfigs();
}