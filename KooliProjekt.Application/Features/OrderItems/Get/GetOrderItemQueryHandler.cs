using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.OrderItems
{
    public class GetOrderItemQueryHandler : IRequestHandler<GetOrderItemQuery, OperationResult<OrderItemDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetOrderItemQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<OrderItemDto>> Handle(GetOrderItemQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<OrderItemDto>();
            result.Value = await _dbContext.OrderItems
                .AsNoTracking()
                .Where(item => item.Id == request.Id)
                .Select(item => new OrderItemDto
                {
                    Id = item.Id,
                    OrderId = item.Order.Id,
                    OrderDate = item.Order.OrderDate,
                    OrderStatus = item.Order.Status,
                    ProductId = item.Product.Id,
                    ProductName = item.Product.Name,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price,
                    LineTotal = item.Price * item.Quantity
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
