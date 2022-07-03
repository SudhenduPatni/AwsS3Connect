using AwsS3Connect.Core.CommandQuery;
using AwsS3Connect.Infra.Interface;
using MediatR;

namespace AwsS3Connect.Domain.Handler
{
    public class UploadFileHandler : IRequestHandler<UploadFileRequest, bool>
    {
        private readonly IAwsS3Repository _awsS3Repository;

        public UploadFileHandler(IAwsS3Repository awsS3Repository)
        {
            _awsS3Repository = awsS3Repository;
        }

        public async Task<bool> Handle(UploadFileRequest request, CancellationToken cancellationToken)
        {
            return await _awsS3Repository.UploadFile(request.File);
        }
    }
}
