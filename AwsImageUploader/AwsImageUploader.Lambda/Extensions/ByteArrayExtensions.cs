namespace AwsImageUploader.Lambda.Extensions
{
    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.PixelFormats;

    public static class ByteArrayExtensions
    {
        public static Image<Rgba32> ToImage(this byte[] array)
        {
            return Image.Load(array);
        }
    }
}
