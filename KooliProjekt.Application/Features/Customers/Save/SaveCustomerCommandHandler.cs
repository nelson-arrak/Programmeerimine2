using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.Application.Infrastructure.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Customers
{
    public class SaveCustomerCommandHandler : IRequestHandler<SaveCustomerCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveCustomerCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveCustomerCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var emailTaken = await _dbContext.Customers
                .AnyAsync(customer => customer.Email == request.Email && customer.Id != request.Id, cancellationToken);
            if (emailTaken)
            {
                return result.AddPropertyError(nameof(request.Email), "Customer with this email already exists");
            }

            var phoneTaken = await _dbContext.Customers
                .AnyAsync(customer => customer.PhoneNumber == request.PhoneNumber && customer.Id != request.Id, cancellationToken);
            if (phoneTaken)
            {
                return result.AddPropertyError(nameof(request.PhoneNumber), "Customer with this phone number already exists");
            }

            var customer = new Customer();

            if (request.Id == 0)
            {
                _dbContext.Customers.Add(customer);
            }
            else
            {
                customer = await _dbContext.Customers.FindAsync(new object[] { request.Id }, cancellationToken);
                if (customer == null)
                {
                    return result.AddError("Cannot find customer with ID " + request.Id);
                }
            }

            customer.Name = request.Name;
            customer.Email = request.Email;
            customer.PhoneNumber = request.PhoneNumber;

            // Parooli hoiame ainult räsina, muutmisel tühi parool tähendab "ära muuda"
            if (!string.IsNullOrEmpty(request.Password))
            {
                customer.PasswordHash = PasswordHasher.Hash(request.Password);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
