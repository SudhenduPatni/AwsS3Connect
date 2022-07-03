using Amazon.S3.Model;
using AwsS3Connect.Core.CommandQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AwsS3Connect.Api.Controllers
{
    [ApiController]
    [Route("[api/controller]")]
    public class AwsS3Controller : ControllerBase
    {
        private readonly IMediator _mediator;

        public AwsS3Controller(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(Name = "GetFile")]
        public async Task<IActionResult> GetFile(GetFileRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Name))
                return ResponseMessage($"File name must required.", (int) HttpStatusCode.BadRequest);

            var bytes = await _mediator.Send(request);

            return File(bytes, "application/octet-stream", request.Name);
        }

        [HttpPost(Name = "GetFiles")]
        public async Task<ListVersionsResponse> GetFiles(GetFilesRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.FilterText))
                request.FilterText = "*";

            return await _mediator.Send(request);
        }

        [HttpPost(Name = "UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file is null || file.Length <= 0)
                return ResponseMessage($"File must required to upload at S3 bucket.", (int)HttpStatusCode.BadRequest);

            var request = new UploadFileRequest
            {
                File = file
            };

            var result = await _mediator.Send(request);

            if (result)
                return ResponseMessage("File successfully uploaded to the S3bucket.", (int) HttpStatusCode.Created);
            else
                return ResponseMessage("Error occurred uploading file at S3 bucket.", (int) HttpStatusCode.InternalServerError);
        }

        [HttpPost(Name = "UpdateFile")]
        public async Task<IActionResult> UpdateFile(UpdateFileRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Name))
                return ResponseMessage($"File name must required to update a file.", (int) HttpStatusCode.BadRequest);

            var result = await _mediator.Send(request);

            if (result)
                return ResponseMessage("File successfully updated.", (int)HttpStatusCode.OK);
            else
                return ResponseMessage("Error occurred updating file.", (int)HttpStatusCode.InternalServerError);
        }

        [HttpPost(Name = "DeleteFile")]
        public async Task<IActionResult> DeleteFile(DeleteFileRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Name))
                return ResponseMessage($"File name must required to delete a file.", (int)HttpStatusCode.BadRequest);

            var result = await _mediator.Send(request);

            if (result)
                return ResponseMessage("File successfully deleted from the S3 bucket.", (int)HttpStatusCode.OK);
            else
                return ResponseMessage("Error occurred deleting file from the S3 bucket.", (int)HttpStatusCode.InternalServerError);
        }

        private IActionResult ResponseMessage(string message, int? statusCode = null)
        {
            return new ContentResult
            {
                Content = message,
                ContentType = "application/json",
                StatusCode = statusCode
            };
        }
    }
}