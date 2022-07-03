using MediatR;

namespace AwsS3Connect.Core.CommandQuery
{
    public class DeleteFileRequest : IRequest<bool>
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
