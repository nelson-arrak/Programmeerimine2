using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Addresses
{
    // Andmete kontroll (kohustuslikud väljad, olemasolu, unikaalsus) on SaveAddressCommandValidator klassis
    public class SaveAddressCommandHandler : IRequestHandler<SaveAddressCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveAddressCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveAddressCommand request, CancellationToken cancellationToken)
        {
            var entity = new Address();

            if (request.Id == 0)
            {
                _dbContext.Addresses.Add(entity);
            }
            else
            {
                entity = await _dbContext.Addresses.FindAsync(new object[] { request.Id }, cancellationToken);
            }

            entity.Street = request.Street;
            entity.City = request.City;
            entity.State = request.State;
            entity.ZipCode = request.ZipCode;
            entity.Country = request.Country;

            // Address klassis pole CustomerId välja, see on EF-i "shadow property"
            _dbContext.Entry(entity).Property("CustomerId").CurrentValue = request.CustomerId;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new OperationResult();
        }
    }
}
