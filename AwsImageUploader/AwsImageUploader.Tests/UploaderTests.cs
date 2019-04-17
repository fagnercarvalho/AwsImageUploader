using System;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using AwsImageUploader.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AwsImageUploader.Tests
{
    [TestClass]
    public class UploaderTests
    {
        private const string BucketName = "test-bucket-henry-upwork";
        private const string ImagePath = @"App_Data\tokyo.jpg";
        private const string AccessKey = "AKIAWYNCPQCLCUVFXQ5X";
        private const string AccessSecret = "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";
        private readonly RegionEndpoint Region = RegionEndpoint.SAEast1;

        [TestMethod]
        public async Task UploadExistingFileShouldBeOk()
        {
            var uploader = new Uploader(AccessKey, AccessSecret, RegionEndpoint.SAEast1);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), ImagePath);

            var guid = Guid.NewGuid().ToString();
            var fileName = await uploader.UploadImageAsync(BucketName, guid, filePath);

            var validUrl = string.Format("https://s3-{0}.amazonaws.com/{1}/{2}", Region.SystemName, BucketName, guid);
            Assert.AreEqual(validUrl, fileName);
        }

        [TestMethod]
        public async Task UploadExistingFileFromStreamShouldBeOk()
        {
            var uploader = new Uploader(AccessKey, AccessSecret, RegionEndpoint.SAEast1);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), ImagePath);

            var guid = Guid.NewGuid().ToString();
            string fileName;
            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                fileName = await uploader.UploadImageAsync(BucketName, guid, fileStream);
            }

            var validUrl = string.Format("https://s3-{0}.amazonaws.com/{1}/{2}", Region.SystemName, BucketName, guid);
            Assert.AreEqual(validUrl, fileName);
        }
    }
}
