namespace RemotePhotographer.Features.Gphoto2.Models
{
    public sealed class Gphoto2Exception
        : Exception
    {
        public Gphoto2Exception(int status, string message) 
            : base(message)
        {
            Status = status;
        }

        public int Status { get; }
    }
}
