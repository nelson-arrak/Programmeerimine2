using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Addresses
{
    // Olemasolu ja sõltuvuste kontroll on DeleteAddressCommandValidator klassis
    public class DeleteAddressCommandHandler : IRequestHandler<DeleteAddressCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteAddressCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
        {
            await _dbContext.Addresses
                .Where(entity => entity.Id == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            return new OperationResult();
        }
    }
}
