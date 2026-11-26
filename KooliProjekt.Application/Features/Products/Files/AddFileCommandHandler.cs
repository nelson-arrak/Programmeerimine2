using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Infrastructure.Files;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Products
{
    public class AddFileCommandHandler : IRequestHandler<AddFileCommand, OperationResult<string>>
    {
        private readonly IFileClient _fileClient;

        public AddFileCommandHandler(IFileClient fileClient)
        {
            _fileClient = fileClient;
        }

        public async Task<OperationResult<string>> Handle(AddFileCommand request, CancellationToken cancellationToken)
        {
            // Kasutaja antud nime ei kasuta - see võib korduda või sisaldada ohtlikke märke.
            // Salvestame unikaalse nimega, laiend jääb samaks.
            var extension = Path.GetExtension(request.FileName).ToLowerInvariant();
            var fileName = Guid.NewGuid().ToString() + extension;

            await _fileClient.Save(fileName, FileStore.ProductPhotos(request.ProductId), request.InputStream);

            return new OperationResult<string>(fileName);
        }
    }
}
