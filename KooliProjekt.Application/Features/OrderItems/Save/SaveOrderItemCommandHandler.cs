using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.OrderItems
{
    public class SaveOrderItemCommandHandler : IRequestHandler<SaveOrderItemCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveOrderItemCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveOrderItemCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var order = await _dbContext.Orders.FindAsync(new object[] { request.OrderId }, cancellationToken);
            if (order == null)
            {
                return result.AddPropertyError(nameof(request.OrderId), "Cannot find order with ID " + request.OrderId);
            }

            var product = await _dbContext.Products.FindAsync(new object[] { request.ProductId }, cancellationToken);
            if (product == null)
            {
                return result.AddPropertyError(nameof(request.ProductId), "Cannot find product with ID " + request.ProductId);
            }

            var item = new OrderItem();

            if (request.Id == 0)
            {
                _dbContext.OrderItems.Add(item);
            }
            else
            {
                item = await _dbContext.OrderItems
                    .Include(orderItem => orderItem.Product)
                    .FirstOrDefaultAsync(orderItem => orderItem.Id == request.Id, cancellationToken);
                if (item == null)
                {
                    return result.AddError("Cannot find order item with ID " + request.Id);
                }
            }

            // Hind võetakse tootelt hetkel, kui toode reale lisatakse.
            // Hiljem toote hinna muutmine ei muuda juba tehtud tellimusi.
            if (item.Product == null || item.Product.Id != product.Id)
            {
                item.Price = product.Price;
            }

            item.Order = order;
            item.Product = product;
            item.Quantity = request.Quantity;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
