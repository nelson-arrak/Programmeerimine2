using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Categories
{
    public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, OperationResult<CategoryDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetCategoryQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<CategoryDto>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<CategoryDto>();
            result.Value = await _dbContext.Category
                .AsNoTracking()
                .Where(category => category.Id == request.Id)
                .Select(category => new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Products = _dbContext.Products
                        .Where(product => product.Category.Id == category.Id)
                        .OrderBy(product => product.Name)
                        .Select(product => new CategoryProductDto
                        {
                            Id = product.Id,
                            Name = product.Name,
                            Price = product.Price,
                            InStock = product.StockQuantity > 0
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
