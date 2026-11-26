using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Categories
{
    public class SaveCategoryCommandHandler : IRequestHandler<SaveCategoryCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveCategoryCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveCategoryCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var nameTaken = await _dbContext.Category
                .AnyAsync(category => category.Name == request.Name && category.Id != request.Id, cancellationToken);
            if (nameTaken)
            {
                return result.AddPropertyError(nameof(request.Name), "Category with this name already exists");
            }

            var category = new Category();

            if (request.Id == 0)
            {
                _dbContext.Category.Add(category);
            }
            else
            {
                category = await _dbContext.Category.FindAsync(new object[] { request.Id }, cancellationToken);
                if (category == null)
                {
                    return result.AddError("Cannot find category with ID " + request.Id);
                }
            }

            category.Name = request.Name;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
