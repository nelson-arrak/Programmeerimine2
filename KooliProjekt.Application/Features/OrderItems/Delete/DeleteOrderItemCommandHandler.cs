using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.OrderItems
{
    // Olemasolu ja sõltuvuste kontroll on DeleteOrderItemCommandValidator klassis
    public class DeleteOrderItemCommandHandler : IRequestHandler<DeleteOrderItemCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteOrderItemCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteOrderItemCommand request, CancellationToken cancellationToken)
        {
            await _dbContext.OrderItems
                .Where(entity => entity.Id == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            return new OperationResult();
        }
    }
}
