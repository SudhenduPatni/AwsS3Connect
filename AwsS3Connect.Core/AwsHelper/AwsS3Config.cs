using Amazon.S3;

namespace AwsS3Connect.Core.AwsHelper
{
    public class AwsS3Config : IAwsS3Config
    {
        public IAmazonS3 GetAwsS3Client()
        {
            return new AmazonS3Client("AccessKey", "SecretAccessKey", Amazon.RegionEndpoint.USEast1);
        }
    }
}
