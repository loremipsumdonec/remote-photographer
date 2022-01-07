namespace RemotePhotographer.Features.Gphoto2.Services
{
    public interface IMethodValidator
    {
        void Validate(int status, string name);
    }
}
