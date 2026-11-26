using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.OrderItems
{
    // Andmete kontroll (olemasolu, kogus, suletud tellimus jne) on SaveOrderItemCommandValidator klassis
    public class SaveOrderItemCommandHandler : IRequestHandler<SaveOrderItemCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveOrderItemCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveOrderItemCommand request, CancellationToken cancellationToken)
        {
            var entity = new OrderItem();

            if (request.Id == 0)
            {
                _dbContext.OrderItems.Add(entity);
            }
            else
            {
                entity = await _dbContext.OrderItems
                    .Include(item => item.Product)
                    .FirstOrDefaultAsync(item => item.Id == request.Id, cancellationToken);
            }

            var product = await _dbContext.Products.FindAsync(new object[] { request.ProductId }, cancellationToken);

            // Hind võetakse tootelt hetkel, kui toode reale lisatakse.
            // Hiljem toote hinna muutmine ei muuda juba tehtud tellimusi.
            if (entity.Product == null || entity.Product.Id != product.Id)
            {
                entity.Price = product.Price;
            }

            entity.Order = await _dbContext.Orders.FindAsync(new object[] { request.OrderId }, cancellationToken);
            entity.Product = product;
            entity.Quantity = request.Quantity;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new OperationResult();
        }
    }
}
