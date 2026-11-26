using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Files;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Products
{
    public class ListFilesQueryHandler : IRequestHandler<ListFilesQuery, OperationResult<IList<string>>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IFileClient _fileClient;

        public ListFilesQueryHandler(ApplicationDbContext dbContext, IFileClient fileClient)
        {
            _dbContext = dbContext;
            _fileClient = fileClient;
        }

        public async Task<OperationResult<IList<string>>> Handle(ListFilesQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<IList<string>>();

            // Olematu toote puhul jääb Value null-iks -> 404, nagu Get päringutel
            var productExists = await _dbContext.Products
                .AnyAsync(product => product.Id == request.ProductId, cancellationToken);
            if (productExists)
            {
                result.Value = await _fileClient.List(FileStore.ProductPhotos(request.ProductId));
            }

            return result;
        }
    }
}
