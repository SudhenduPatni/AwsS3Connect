using Amazon.S3.Model;
using MediatR;

namespace AwsS3Connect.Core.CommandQuery
{
    public class GetFilesRequest : IRequest<ListVersionsResponse>
    {
        public string FilterText { get; set; } = "*";
    }
}
