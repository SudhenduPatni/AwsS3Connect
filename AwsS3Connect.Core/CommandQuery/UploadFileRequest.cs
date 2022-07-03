using MediatR;
using Microsoft.AspNetCore.Http;

namespace AwsS3Connect.Core.CommandQuery
{
    public class UploadFileRequest : IRequest<bool>
    {
        public IFormFile File { get; set; }
    }
}
