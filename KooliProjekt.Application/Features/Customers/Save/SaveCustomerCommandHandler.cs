using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.Application.Infrastructure.Security;
using MediatR;

namespace KooliProjekt.Application.Features.Customers
{
    // Andmete kontroll (kohustuslikud väljad, olemasolu, unikaalsus) on SaveCustomerCommandValidator klassis
    public class SaveCustomerCommandHandler : IRequestHandler<SaveCustomerCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveCustomerCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveCustomerCommand request, CancellationToken cancellationToken)
        {
            var entity = new Customer();

            if (request.Id == 0)
            {
                _dbContext.Customers.Add(entity);
            }
            else
            {
                entity = await _dbContext.Customers.FindAsync(new object[] { request.Id }, cancellationToken);
            }

            entity.Name = request.Name;
            entity.Email = request.Email;
            entity.PhoneNumber = request.PhoneNumber;

            // Parooli hoiame ainult räsina, muutmisel tühi parool tähendab "ära muuda"
            if (!string.IsNullOrEmpty(request.Password))
            {
                entity.PasswordHash = PasswordHasher.Hash(request.Password);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new OperationResult();
        }
    }
}
