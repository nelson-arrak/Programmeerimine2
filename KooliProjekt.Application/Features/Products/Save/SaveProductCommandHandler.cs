using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Products
{
    public class SaveProductCommandHandler : IRequestHandler<SaveProductCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveProductCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveProductCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var category = await _dbContext.Category.FindAsync(new object[] { request.CategoryId }, cancellationToken);
            if (category == null)
            {
                return result.AddPropertyError(nameof(request.CategoryId), "Cannot find category with ID " + request.CategoryId);
            }

            var nameTaken = await _dbContext.Products
                .AnyAsync(product => product.Name == request.Name && product.Id != request.Id, cancellationToken);
            if (nameTaken)
            {
                return result.AddPropertyError(nameof(request.Name), "Product with this name already exists");
            }

            var product = new Product();

            if (request.Id == 0)
            {
                _dbContext.Products.Add(product);
            }
            else
            {
                product = await _dbContext.Products.FindAsync(new object[] { request.Id }, cancellationToken);
                if (product == null)
                {
                    return result.AddError("Cannot find product with ID " + request.Id);
                }
            }

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.StockQuantity = request.StockQuantity;
            product.Category = category;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
