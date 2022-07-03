using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using AwsS3Connect.Core.AwsHelper;
using AwsS3Connect.Infra.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace AwsS3Connect.Infra.Repository
{
    public class AwsS3Repository : IAwsS3Repository
    {
        private readonly IConfiguration _configuration;
        private readonly IAmazonS3 _awsS3Client;
        private readonly IAwsS3Config _awsS3Config;
        private readonly string _bucketName;

        public AwsS3Repository(IConfiguration configuration, IAwsS3Config awsS3Config, IAmazonS3 awsS3Client)
        {
            _configuration = configuration;
            _awsS3Config = awsS3Config;
            _awsS3Client = awsS3Client;

            _bucketName = _configuration["AwsS3Settings:BucketName"];
        }

        public async Task<byte[]> GetFile(string fileName)
        {
            MemoryStream stream = null;

            using (var response = await _awsS3Client.GetObjectAsync(AwsS3Request.S3GetObjectRequest(_bucketName, fileName)))
            {
                if (response.HttpStatusCode == HttpStatusCode.OK)
                {
                    using (stream = new MemoryStream())
                    {
                        await response.ResponseStream.CopyToAsync(stream);
                    }
                }
            }

            return stream.ToArray();
        }

        public async Task<ListVersionsResponse> GetFiles(string filterText)
        {
            var result = await _awsS3Client.ListVersionsAsync(_bucketName);

            if (string.Equals(filterText, "*"))
            {
                return result; // TODO: Need to return just a list files from the bucket
            }
            else
            {
                return result; // TODO: Need to filter files based on search filter text
            }
        }

        public async Task<bool> UploadFile(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);

                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = memoryStream,
                    Key = file.FileName,
                    BucketName = _bucketName,
                    ContentType = file.ContentType
                };

                var fileTransferUtility = new TransferUtility(_awsS3Client);
                await fileTransferUtility.UploadAsync(uploadRequest);

                return true;
            }
        }

        public async Task<bool> UpdateFile(string fileName)
        {
            if (!IsFileExists(fileName, ""))
                throw new FileNotFoundException(String.Format($"File '{fileName}' is not found."));

            var deleted = await DeleteFile(fileName);

            if (deleted)
            {
                await UpdateFile(fileName);

                return true;
            }
            else 
                return false;
        }

        public async Task<bool> DeleteFile(string fileName, string versionId = "")
        {
            if (!IsFileExists(fileName, versionId))
                throw new FileNotFoundException(String.Format($"File '{fileName}' is not found."));

            if (string.IsNullOrEmpty(versionId))
            {
                await DeleteFileWithVersion(fileName, versionId);

                return true;
            }

            var listVersionRequest = new ListVersionsRequest { BucketName = _bucketName, Prefix = fileName };
            var listVersionResponse = await _awsS3Client.ListVersionsAsync(listVersionRequest);

            foreach (S3ObjectVersion versionIds in listVersionResponse.Versions)
            {
                if (versionIds.VersionId == versionId)
                {
                    await DeleteFileWithVersion(fileName, versionIds.VersionId);
                }
            }

            return true;
        }

        private async Task DeleteFileWithVersion(string fileName, string versionId)
        {
            DeleteObjectRequest request = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName
            };

            if (!string.IsNullOrEmpty(versionId))
                request.VersionId = versionId;

            await _awsS3Client.DeleteObjectAsync(request);
        }

        private bool IsFileExists(string fileName, string versionId)
        {
            try
            {
                GetObjectMetadataRequest request = new GetObjectMetadataRequest
                {
                    BucketName = _bucketName,
                    Key = fileName,
                    VersionId = !string.IsNullOrEmpty(versionId) ? versionId : null
                };

                var response = _awsS3Client.GetObjectMetadataAsync(request).Result;

                if (response == null)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                if (ex.InnerException is AmazonS3Exception awsEx)
                {
                    if (string.Equals(awsEx.ErrorCode, "NoSuchBucket"))
                        return false;
                    else if (string.Equals(awsEx.ErrorCode, "NotFound"))
                        return false;
                }

                throw;
            }
        }
    }
}
