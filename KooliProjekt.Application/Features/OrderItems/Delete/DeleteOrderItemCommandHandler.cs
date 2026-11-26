using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.OrderItems
{
    public class DeleteOrderItemCommandHandler : IRequestHandler<DeleteOrderItemCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteOrderItemCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteOrderItemCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var exists = await _dbContext.OrderItems
                .AnyAsync(entity => entity.Id == request.Id, cancellationToken);
            if (!exists)
            {
                return result.AddError("Cannot find order item with ID " + request.Id);
            }

            await _dbContext.OrderItems
                .Where(entity => entity.Id == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            return result;
        }
    }
}
