namespace RemotePhotographer.Features.Gphoto2.Services
{
    public interface IMethodValidator
    {
        int Validate(int status, string name);
    }
}
