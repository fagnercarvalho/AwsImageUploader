using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AwsImageUploader.Lambda
{
    using Amazon;
    using Amazon.Lambda.APIGatewayEvents;
    using AwsImageUploader.Lambda.Extensions;
    using SixLabors.ImageSharp.Formats;
    using System;
    using System.Collections.Generic;
    using System.Net;

    public class Function
    {
        private const string BucketName = "test-bucket-henry-upwork";
        private static readonly RegionEndpoint Region = RegionEndpoint.SAEast1;

        /// <summary>
        ///     A function that takes an image filename and a resolution and returns a resized image
        /// </summary>
        /// <param name="input">AWS Lambda service API Gateway request.</param>
        /// <param name="context">AWS Lambda service context.</param>
        /// <returns></returns>
        public APIGatewayProxyResponse FunctionHandler(
            APIGatewayProxyRequest input, 
            ILambdaContext context)
        {
            // preparing parameters
            var keyName = input.QueryStringParameters["keyName"];
            var mimeType = MimeTypes.GetMimeType(keyName);
            var headersDic = new Dictionary<string, string>() { { "Content-type", mimeType } };

            // requesting image from AWS S3 bucket
            var webClient = new WebClient();
            var imageUrl = string.Format("https://s3-{0}.amazonaws.com/{1}/{2}", Region.SystemName, BucketName, keyName);
            var imageBytes = webClient.DownloadData(imageUrl);

            // resize image
            var resolution = input.QueryStringParameters["resolution"];
            var image = imageBytes.ToImage();
            var size = resolution.Split("x");
            var encoder = this.GetImageEncoder(mimeType);
            var resizedImage = image.ResizeImage(int.Parse(size[0]), int.Parse(size[1]), encoder);
            var resizedImageBytes = resizedImage.ToByteArray(encoder);

            return new APIGatewayProxyResponse
            {
                StatusCode = 200,
                Headers = headersDic,
                Body = Convert.ToBase64String(resizedImageBytes),
                IsBase64Encoded = true
            };
        }

        /// <summary>
        ///     Get right SixLabors.ImageSharp library image encoder based on image MIME type.
        /// </summary>
        /// <param name="mimeType">Image MIME type.</param>
        /// <returns>SixLabors.ImageSharp encoder.</returns>
        private IImageEncoder GetImageEncoder(string mimeType)
        {
            switch (mimeType)
            {
                case "image/png":
                    return new SixLabors.ImageSharp.Formats.Png.PngEncoder();
                case "image/gif":
                    return new SixLabors.ImageSharp.Formats.Gif.GifEncoder();
                case "image/bmp":
                    return new SixLabors.ImageSharp.Formats.Bmp.BmpEncoder();
                case "image/jpg":
                case "image/jpeg":
                    return new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder();
                default:
                    return new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder();
            }
        }
    }
}
