using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Orders
{
    public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, OperationResult<OrderDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetOrderQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<OrderDto>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<OrderDto>();
            result.Value = await _dbContext.Orders
                .AsNoTracking()
                .Where(order => order.Id == request.Id)
                .Select(order => new OrderDto
                {
                    Id = order.Id,
                    OrderDate = order.OrderDate,
                    Status = order.Status,
                    CustomerId = order.Customer.Id,
                    CustomerName = order.Customer.Name,
                    CustomerEmail = order.Customer.Email,
                    TotalAmount = order.OrderItems.Sum(item => item.Price * item.Quantity),
                    IsPaid = _dbContext.Payments.Any(payment => payment.OrderId == order.Id && payment.Status),
                    Items = order.OrderItems.Select(item => new OrderLineDto
                    {
                        Id = item.Id,
                        ProductId = item.Product.Id,
                        ProductName = item.Product.Name,
                        Quantity = item.Quantity,
                        UnitPrice = item.Price,
                        LineTotal = item.Price * item.Quantity
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
