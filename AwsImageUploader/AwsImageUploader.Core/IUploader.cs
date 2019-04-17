using System.IO;
using System.Threading.Tasks;

namespace AwsImageUploader.Core
{
    public interface IUploader
    {
        Task<string> UploadImageAsync(string bucketName, string keyName, string filePath);
        Task<string> UploadImageAsync(string bucketName, string keyName, Stream stream);
    }
}
