using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Customers
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteCustomerCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var exists = await _dbContext.Customers
                .AnyAsync(entity => entity.Id == request.Id, cancellationToken);
            if (!exists)
            {
                return result.AddError("Cannot find customer with ID " + request.Id);
            }

            var hasOrders = await _dbContext.Orders
                .AnyAsync(order => order.Customer.Id == request.Id, cancellationToken);
            if (hasOrders)
            {
                return result.AddError("Cannot delete customer because the customer has orders");
            }

            // Aadressid kuuluvad kliendile, seega kustutame need koos kliendiga
            await _dbContext.Addresses
                .Where(address => EF.Property<int?>(address, "CustomerId") == request.Id)
                .ExecuteDeleteAsync(cancellationToken);
            await _dbContext.Customers
                .Where(entity => entity.Id == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            return result;
        }
    }
}
