using AwsS3Connect.Core.CommandQuery;
using AwsS3Connect.Infra.Interface;
using MediatR;

namespace AwsS3Connect.Domain.Handler
{
    public class DeleteFileHandler : IRequestHandler<DeleteFileRequest, bool>
    {
        private readonly IAwsS3Repository _awsS3Repository;

        public DeleteFileHandler(IAwsS3Repository awsS3Repository)
        {
            _awsS3Repository = awsS3Repository;
        }

        public async Task<bool> Handle(DeleteFileRequest request, CancellationToken cancellationToken)
        {
            return await _awsS3Repository.DeleteFile(request.Name);
        }
    }
}
