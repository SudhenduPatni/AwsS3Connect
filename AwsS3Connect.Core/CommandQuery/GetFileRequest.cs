using MediatR;

namespace AwsS3Connect.Core.CommandQuery
{
    public class GetFileRequest : IRequest<byte[]>
    {
        public string Name { get; set; }
    }
}
