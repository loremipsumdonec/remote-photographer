using RemotePhotographer.Features.Gphoto2.Models;

namespace RemotePhotographer.Features.Gphoto2.Services
{
    public class DefaultMethodValidator
        : IMethodValidator
    {
        public int Validate(int status, string name) 
        {
            if(status > -1)
            {
                return status;
            }

            string defaultMessage = $"method {name} failed with status {status}";

            switch(status) 
            {
                case -2:
                    throw new Gphoto2Exception(status, $"{defaultMessage}, bad parameters.");
                case -52:
                    throw new Gphoto2Exception(status, $"{defaultMessage}, error when trying to find USB device.");
                case -53:
                    throw new Gphoto2Exception(status, $"{defaultMessage}, error when trying to claim the USB device.");
                case -105:
                    throw new Gphoto2Exception(status, $"{defaultMessage}, specified camera model was not found.");
                case -108:
                    throw new Gphoto2Exception(status, $"{defaultMessage}, specified file was not found.");
                case -110:
                    throw new Gphoto2Exception(status, $"{defaultMessage}, the camera is already busy.");
                default:
                    throw new Gphoto2Exception(status, defaultMessage);
            }
        }
    }
}
