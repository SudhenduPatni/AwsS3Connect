using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;

namespace AwsS3Connect.Infra.Interface
{
    public interface IAwsS3Repository
    {
        Task<byte[]> GetFile(string fileName);
        Task<ListVersionsResponse> GetFiles(string filterText);
        Task<bool> UploadFile(IFormFile file);
        Task<bool> UpdateFile(string fileName);
        Task<bool> DeleteFile(string fileName, string versionId = "");
    }
}
