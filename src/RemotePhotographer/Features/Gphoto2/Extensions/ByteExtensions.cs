using System.Text;

namespace RemotePhotographer.Features.Gphoto2.Extensions;

public static class ByteExtensions 
{
    public static string ConvertToString(this sbyte[] bytes) 
    {
        string returnStr;

        unsafe
        {   
            fixed(sbyte* fixedPtr = bytes)
            {
                returnStr = new string(fixedPtr);
            }
        }

        return (returnStr);
    }

    public static sbyte[] ConvertToSByte(this string data) 
    {
        return Array.ConvertAll(Encoding.ASCII.GetBytes(data), b => Convert.ToSByte(b));
    }
}