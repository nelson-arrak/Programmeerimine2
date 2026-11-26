using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Orders
{
    // Andmete kontroll (kohustuslikud väljad, olemasolu, unikaalsus) on SaveOrderCommandValidator klassis
    public class SaveOrderCommandHandler : IRequestHandler<SaveOrderCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveOrderCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveOrderCommand request, CancellationToken cancellationToken)
        {
            var entity = new Order();

            if (request.Id == 0)
            {
                _dbContext.Orders.Add(entity);
            }
            else
            {
                entity = await _dbContext.Orders.FindAsync(new object[] { request.Id }, cancellationToken);
            }

            entity.Customer = await _dbContext.Customers.FindAsync(new object[] { request.CustomerId }, cancellationToken);
            entity.OrderDate = request.OrderDate;
            entity.Status = request.Status;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new OperationResult();
        }
    }
}
