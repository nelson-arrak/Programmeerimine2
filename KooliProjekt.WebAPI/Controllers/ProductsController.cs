using System.Threading.Tasks;
using KooliProjekt.Application.Features.Products;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace KooliProjekt.WebAPI.Controllers
{
    public class ProductsController : ApiControllerBase
    {
        private static readonly FileExtensionContentTypeProvider _contentTypeProvider = new FileExtensionContentTypeProvider();
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetProductQuery { Id = id };
            var result = await _mediator.Send(query);

            return Result(result);
        }

        [HttpGet("list/{page}/{pageSize}")]
        public async Task<IActionResult> List(int page, int pageSize)
        {
            var query = new ListProductsQuery
            {
                Page = page,
                PageSize = pageSize
            };
            var result = await _mediator.Send(query);

            return Result(result);
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] SaveProductCommand command)
        {
            var result = await _mediator.Send(command);

            return Result(result);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteProductCommand command)
        {
            var result = await _mediator.Send(command);

            return Result(result);
        }

        [HttpPost("addfile/{id}")]
        public async Task<IActionResult> AddFile(int id, IFormFile file)
        {
            // Faili puudumist ei kontrolli siin - selle teeb AddFileCommandValidator
            using var inputStream = file?.OpenReadStream();

            var command = new AddFileCommand
            {
                ProductId = id,
                FileName = file?.FileName,
                FileSize = file?.Length ?? 0,
                InputStream = inputStream
            };
            var result = await _mediator.Send(command);

            return Result(result);
        }

        [HttpGet("getfile/{id}/{fileName}")]
        public async Task<IActionResult> GetFile(int id, string fileName)
        {
            var command = new GetFileCommand
            {
                ProductId = id,
                FileName = fileName
            };
            var result = await _mediator.Send(command);

            if (result.HasErrors || result.Value == null)
            {
                return Result(result);
            }

            if (!_contentTypeProvider.TryGetContentType(result.Value.FileName, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            // File() sulgeb streami ise pärast vastuse saatmist
            return File(result.Value.Content, contentType);
        }

        [HttpGet("listfiles/{id}")]
        public async Task<IActionResult> ListFiles(int id)
        {
            var query = new ListFilesQuery { ProductId = id };
            var result = await _mediator.Send(query);

            return Result(result);
        }

        [HttpDelete("deletefile")]
        public async Task<IActionResult> DeleteFile([FromBody] DeleteFileCommand command)
        {
            var result = await _mediator.Send(command);

            return Result(result);
        }
    }
}
