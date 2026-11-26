using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Files;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Products
{
    // Olemasolu ja sõltuvuste kontroll on DeleteProductCommandValidator klassis
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IFileClient _fileClient;

        public DeleteProductCommandHandler(ApplicationDbContext dbContext, IFileClient fileClient)
        {
            _dbContext = dbContext;
            _fileClient = fileClient;
        }

        public async Task<OperationResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            await _dbContext.Products
                .Where(entity => entity.Id == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            // Andmebaasis piltide kohta midagi pole, aga toote kaust tuleb samuti ära kustutada
            await _fileClient.DeleteStore(FileStore.ProductPhotos(request.Id));

            return new OperationResult();
        }
    }
}
