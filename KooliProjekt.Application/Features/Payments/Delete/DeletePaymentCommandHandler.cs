using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Payments
{
    // Olemasolu ja sõltuvuste kontroll on DeletePaymentCommandValidator klassis
    public class DeletePaymentCommandHandler : IRequestHandler<DeletePaymentCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeletePaymentCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeletePaymentCommand request, CancellationToken cancellationToken)
        {
            await _dbContext.Payments
                .Where(entity => entity.Id == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            return new OperationResult();
        }
    }
}
