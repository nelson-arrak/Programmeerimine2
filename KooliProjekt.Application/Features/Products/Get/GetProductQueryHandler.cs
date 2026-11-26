using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Products
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, OperationResult<ProductDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetProductQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<ProductDto>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<ProductDto>();
            result.Value = await _dbContext.Products
                .AsNoTracking()
                .Where(product => product.Id == request.Id)
                .Select(product => new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    StockQuantity = product.StockQuantity,
                    InStock = product.StockQuantity > 0,
                    CategoryId = product.Category.Id,
                    CategoryName = product.Category.Name
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
