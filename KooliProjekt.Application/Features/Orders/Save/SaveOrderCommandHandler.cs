using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Orders
{
    public class SaveOrderCommandHandler : IRequestHandler<SaveOrderCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveOrderCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveOrderCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var customer = await _dbContext.Customers.FindAsync(new object[] { request.CustomerId }, cancellationToken);
            if (customer == null)
            {
                return result.AddPropertyError(nameof(request.CustomerId), "Cannot find customer with ID " + request.CustomerId);
            }

            var order = new Order();

            if (request.Id == 0)
            {
                _dbContext.Orders.Add(order);
            }
            else
            {
                order = await _dbContext.Orders.FindAsync(new object[] { request.Id }, cancellationToken);
                if (order == null)
                {
                    return result.AddError("Cannot find order with ID " + request.Id);
                }
            }

            order.Customer = customer;
            order.OrderDate = request.OrderDate;
            order.Status = request.Status;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
