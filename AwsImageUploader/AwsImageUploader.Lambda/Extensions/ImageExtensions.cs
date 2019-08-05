namespace AwsImageUploader.Lambda.Extensions
{
    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.Formats;
    using SixLabors.ImageSharp.PixelFormats;
    using SixLabors.ImageSharp.Processing;
    using System.IO;

    public static class ImageExtensions
    {
        public static byte[] ToByteArray(this Image<Rgba32> imageIn, IImageEncoder encoder)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, encoder);
                return ms.ToArray();
            }
        }

        public static Image<Rgba32> ResizeImage(this Image<Rgba32> imgToResize, int width, int height, IImageEncoder encoder)
        {
            var imageBuffer = new MemoryStream();

            var resizeOptions = new ResizeOptions
            {
                Size = new SixLabors.Primitives.Size { Width = width, Height = height },
                Mode = ResizeMode.Stretch
            };
            imgToResize.Mutate(x => x.Resize(resizeOptions));
            imgToResize.Save(imageBuffer, encoder);

            imageBuffer.Position = 0;

            var resizedImage = Image.Load(imageBuffer);
            return resizedImage;
        }
    }
}
