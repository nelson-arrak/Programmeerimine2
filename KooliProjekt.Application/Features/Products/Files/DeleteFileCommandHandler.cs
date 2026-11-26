using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Infrastructure.Files;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Products
{
    public class DeleteFileCommandHandler : IRequestHandler<DeleteFileCommand, OperationResult>
    {
        private readonly IFileClient _fileClient;

        public DeleteFileCommandHandler(IFileClient fileClient)
        {
            _fileClient = fileClient;
        }

        public async Task<OperationResult> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
        {
            await _fileClient.Delete(request.FileName, FileStore.ProductPhotos(request.ProductId));

            return new OperationResult();
        }
    }
}
