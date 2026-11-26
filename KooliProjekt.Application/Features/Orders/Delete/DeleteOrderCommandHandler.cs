using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Orders
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteOrderCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var exists = await _dbContext.Orders
                .AnyAsync(entity => entity.Id == request.Id, cancellationToken);
            if (!exists)
            {
                return result.AddError("Cannot find order with ID " + request.Id);
            }

            await _dbContext.Payments
                .Where(payment => payment.OrderId == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            await _dbContext.OrderItems
                .Where(item => item.Order.Id == request.Id)
                .ExecuteDeleteAsync(cancellationToken);
            await _dbContext.Orders
                .Where(entity => entity.Id == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            return result;
        }
    }
}
