using Amazon.S3;

namespace AwsS3Connect.Core.AwsHelper
{
    public interface IAwsS3Config
    {
        IAmazonS3 GetAwsS3Client();
    }
}
