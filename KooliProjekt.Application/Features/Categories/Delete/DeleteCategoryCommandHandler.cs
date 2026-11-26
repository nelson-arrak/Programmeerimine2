using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Categories
{
    // Olemasolu ja sõltuvuste kontroll on DeleteCategoryCommandValidator klassis
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteCategoryCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            await _dbContext.Category
                .Where(entity => entity.Id == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            return new OperationResult();
        }
    }
}
