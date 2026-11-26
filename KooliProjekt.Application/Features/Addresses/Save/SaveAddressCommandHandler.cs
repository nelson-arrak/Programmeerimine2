using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Addresses
{
    public class SaveAddressCommandHandler : IRequestHandler<SaveAddressCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveAddressCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveAddressCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            if (request.CustomerId.HasValue)
            {
                var customerExists = await _dbContext.Customers
                    .AnyAsync(customer => customer.Id == request.CustomerId, cancellationToken);
                if (!customerExists)
                {
                    return result.AddPropertyError(nameof(request.CustomerId), "Cannot find customer with ID " + request.CustomerId);
                }
            }

            var address = new Address();

            if (request.Id == 0)
            {
                _dbContext.Addresses.Add(address);
            }
            else
            {
                address = await _dbContext.Addresses.FindAsync(new object[] { request.Id }, cancellationToken);
                if (address == null)
                {
                    return result.AddError("Cannot find address with ID " + request.Id);
                }
            }

            address.Street = request.Street;
            address.City = request.City;
            address.State = request.State;
            address.ZipCode = request.ZipCode;
            address.Country = request.Country;

            // Address klassis pole CustomerId välja, see on EF-i "shadow property"
            _dbContext.Entry(address).Property("CustomerId").CurrentValue = request.CustomerId;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
