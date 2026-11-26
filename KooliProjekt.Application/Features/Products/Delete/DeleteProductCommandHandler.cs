using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Products
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteProductCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var exists = await _dbContext.Products
                .AnyAsync(entity => entity.Id == request.Id, cancellationToken);
            if (!exists)
            {
                return result.AddError("Cannot find product with ID " + request.Id);
            }

            var isOrdered = await _dbContext.OrderItems
                .AnyAsync(item => item.Product.Id == request.Id, cancellationToken);
            if (isOrdered)
            {
                return result.AddError("Cannot delete product because it is used in orders");
            }
            await _dbContext.Products
                .Where(entity => entity.Id == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            return result;
        }
    }
}
