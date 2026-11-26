using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Products
{
    // Andmete kontroll (kohustuslikud väljad, olemasolu, unikaalsus) on SaveProductCommandValidator klassis
    public class SaveProductCommandHandler : IRequestHandler<SaveProductCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveProductCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveProductCommand request, CancellationToken cancellationToken)
        {
            var entity = new Product();

            if (request.Id == 0)
            {
                _dbContext.Products.Add(entity);
            }
            else
            {
                entity = await _dbContext.Products.FindAsync(new object[] { request.Id }, cancellationToken);
            }

            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.Price = request.Price;
            entity.StockQuantity = request.StockQuantity;
            entity.Category = await _dbContext.Category.FindAsync(new object[] { request.CategoryId }, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new OperationResult();
        }
    }
}
