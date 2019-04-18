
using Amazon;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using AwsImageUploader.Lambda.Extensions;
using SixLabors.ImageSharp.Formats;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Mime;
using static System.Net.Mime.MediaTypeNames;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AwsImageUploader.Lambda
{
    public class Function
    {
        private const string BucketName = "test-bucket-henry-upwork";
        private static readonly RegionEndpoint Region = RegionEndpoint.SAEast1;

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="resolution"></param>
        /// <param name="keyName"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public APIGatewayProxyResponse FunctionHandler(
            APIGatewayProxyRequest input, 
            ILambdaContext context)
        {
            var keyName = input.QueryStringParameters["keyName"];
            var mimeType = MimeTypes.GetMimeType(keyName);
            var headersDic = new Dictionary<string, string>() { { "Content-type", mimeType } };

            var webClient = new WebClient();
            var imageUrl = string.Format("https://s3-{0}.amazonaws.com/{1}/{2}", Region.SystemName, BucketName, keyName);
            var imageBytes = webClient.DownloadData(imageUrl);

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
                    return null;
            }
        }
    }
}
