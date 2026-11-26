using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Addresses
{
    public class DeleteAddressCommandHandler : IRequestHandler<DeleteAddressCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteAddressCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var exists = await _dbContext.Addresses
                .AnyAsync(entity => entity.Id == request.Id, cancellationToken);
            if (!exists)
            {
                return result.AddError("Cannot find address with ID " + request.Id);
            }

            await _dbContext.Addresses
                .Where(entity => entity.Id == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            return result;
        }
    }
}
