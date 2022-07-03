namespace AwsS3Connect.Core.Models
{
    public class AwsS3File
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Data { get; set; }
        public string S3BucketName { get; set; }
        public string S3BucketFilePath { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
