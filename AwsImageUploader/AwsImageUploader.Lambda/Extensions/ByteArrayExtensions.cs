using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace AwsImageUploader.Lambda.Extensions
{
    public static class ByteArrayExtensions
    {
        public static Image<Rgba32> ToImage(this byte[] array)
        {
            return Image.Load(array);
        }
    }
}
