namespace AwsImageUploader.Core
{
    using Amazon;
    using Amazon.S3;
    using Amazon.S3.Transfer;
    using System.IO;
    using System.Threading.Tasks;

    public class Uploader : IUploader
    {
        private readonly IAmazonS3 s3Client;
        private readonly RegionEndpoint region;

        public Uploader(string accessKey, string secretKey)
        {
            this.region = RegionEndpoint.USWest2;
            this.s3Client = new AmazonS3Client(accessKey, secretKey, this.region);
        }

        public Uploader(string accessKey, string secretKey, RegionEndpoint bucketRegion)
        {
            this.region = bucketRegion;
            this.s3Client = new AmazonS3Client(accessKey, secretKey, this.region);
        }

        public async Task<string> UploadImageAsync(
            string bucketName, 
            string keyName, 
            string filePath)
        {
            var fileTransferUtility =
                    new TransferUtility(s3Client);

            var fileTransferUtilityRequest = new TransferUtilityUploadRequest
            {
                BucketName = bucketName,
                FilePath = filePath,
                Key = keyName,
                CannedACL = S3CannedACL.PublicRead
            };
            
            await fileTransferUtility.UploadAsync(fileTransferUtilityRequest);

            return string.Format("https://s3-{0}.amazonaws.com/{1}/{2}", region.SystemName, bucketName, keyName);
        }

        public async Task<string> UploadImageAsync(
            string bucketName, 
            string keyName, 
            Stream stream)
        {
            var fileTransferUtility =
                    new TransferUtility(s3Client);

            var fileTransferUtilityRequest = new TransferUtilityUploadRequest
            {
                BucketName = bucketName,
                InputStream = stream,
                Key = keyName,
                CannedACL = S3CannedACL.PublicRead
            };

            await fileTransferUtility.UploadAsync(fileTransferUtilityRequest);

            return string.Format("https://s3-{0}.amazonaws.com/{1}/{2}", region.SystemName, bucketName, keyName);
        }
    }
}
