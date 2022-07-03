using Amazon.S3.Model;
using AwsS3Connect.Core.CommandQuery;
using AwsS3Connect.Infra.Interface;
using MediatR;

namespace AwsS3Connect.Domain.Handler
{
    public class GetFilesFileHandler : IRequestHandler<GetFilesRequest, ListVersionsResponse>
    {
        private readonly IAwsS3Repository _awsS3Repository;

        public GetFilesFileHandler(IAwsS3Repository awsS3Repository)
        {
            _awsS3Repository = awsS3Repository;
        }

        public async Task<ListVersionsResponse> Handle(GetFilesRequest request, CancellationToken cancellationToken)
        {
            return await _awsS3Repository.GetFiles(request.FilterText);
        }
    }
}
