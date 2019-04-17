using Amazon;
using AwsImageUploader.Core;
using System.IO;
using System.Threading.Tasks;
using static System.Console;

namespace AwsImageUploader.Console
{
    public class Program
    {
        private const string BucketName = "test-bucket-henry-upwork";
        private const string ImagePath = @"App_Data\tokyo.jpg";
        private const string AccessKey = "AKIAWYNCPQCLCUVFXQ5X";
        private const string AccessSecret = "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";
        private static readonly RegionEndpoint Region = RegionEndpoint.SAEast1;

        public static async Task Main(string[] args)
        {
            var uploader = new Uploader(AccessKey, AccessSecret, Region);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), ImagePath);

            var fileName = await uploader.UploadImageAsync(BucketName, Path.GetFileName(filePath), filePath);

            WriteLine("File uploaded successfully!");
            WriteLine($"URL: {fileName}");
            Read();
        }
    }
}
