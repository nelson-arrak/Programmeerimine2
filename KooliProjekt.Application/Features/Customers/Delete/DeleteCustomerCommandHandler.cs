using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Customers
{
    // Olemasolu ja sõltuvuste kontroll on DeleteCustomerCommandValidator klassis
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteCustomerCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            // Aadressid kuuluvad kliendile, seega kustutame need koos kliendiga
            await _dbContext.Addresses
                .Where(address => EF.Property<int?>(address, "CustomerId") == request.Id)
                .ExecuteDeleteAsync(cancellationToken);
            await _dbContext.Customers
                .Where(entity => entity.Id == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            return new OperationResult();
        }
    }
}
