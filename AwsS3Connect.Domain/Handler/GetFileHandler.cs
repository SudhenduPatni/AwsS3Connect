using AwsS3Connect.Core.CommandQuery;
using AwsS3Connect.Infra.Interface;
using MediatR;

namespace AwsS3Connect.Domain.Handler
{
    public class GetFileHandler : IRequestHandler<GetFileRequest, byte[]>
    {
        private readonly IAwsS3Repository _awsS3Repository;

        public GetFileHandler(IAwsS3Repository awsS3Repository)
        {
            _awsS3Repository = awsS3Repository;
        }

        public async Task<byte[]> Handle(GetFileRequest request, CancellationToken cancellationToken)
        {
            return await _awsS3Repository.GetFile(request.Name);
        }
    }
}
