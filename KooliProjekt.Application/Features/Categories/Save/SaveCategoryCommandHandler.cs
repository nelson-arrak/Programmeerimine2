using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Categories
{
    // Andmete kontroll (kohustuslikud väljad, olemasolu, unikaalsus) on SaveCategoryCommandValidator klassis
    public class SaveCategoryCommandHandler : IRequestHandler<SaveCategoryCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveCategoryCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = new Category();

            if (request.Id == 0)
            {
                _dbContext.Category.Add(entity);
            }
            else
            {
                entity = await _dbContext.Category.FindAsync(new object[] { request.Id }, cancellationToken);
            }

            entity.Name = request.Name;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new OperationResult();
        }
    }
}
