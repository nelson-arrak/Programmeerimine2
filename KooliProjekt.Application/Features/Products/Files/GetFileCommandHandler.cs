using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Files;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Products
{
    public class GetFileCommandHandler : IRequestHandler<GetFileCommand, OperationResult<FileContentDto>>
    {
        private readonly IFileClient _fileClient;

        public GetFileCommandHandler(IFileClient fileClient)
        {
            _fileClient = fileClient;
        }

        public async Task<OperationResult<FileContentDto>> Handle(GetFileCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<FileContentDto>();

            var stream = await _fileClient.Get(request.FileName, FileStore.ProductPhotos(request.ProductId));
            if (stream != null)
            {
                result.Value = new FileContentDto
                {
                    FileName = request.FileName,
                    Content = stream
                };
            }

            // Kui faili pole, jääb Value null-iks ja kontroller tagastab 404
            return result;
        }
    }
}
