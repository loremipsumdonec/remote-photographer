using RemotePhotographer.Features.Gphoto2.Models;

namespace RemotePhotographer.Features.Gphoto2.Services
{
    public class DefaultMethodValidator
        : IMethodValidator
    {
        public void Validate(int status, string name) 
        {
            if(status == 0)
            {
                return;
            }

            switch(status) 
            {
                default:
                    throw new Gphoto2Exception(status, $"method {name} failed with status {status}");
            }
        }
    }
}
