using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Orders
{
    public class ListOrdersQueryHandler : IRequestHandler<ListOrdersQuery, OperationResult<PagedResult<OrderListItemDto>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListOrdersQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<OrderListItemDto>>> Handle(ListOrdersQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<OrderListItemDto>>();
            result.Value = await _dbContext.Orders
                .AsNoTracking()
                .OrderByDescending(order => order.OrderDate)
                .ThenByDescending(order => order.Id)
                .Select(order => new OrderListItemDto
                {
                    Id = order.Id,
                    OrderDate = order.OrderDate,
                    Status = order.Status,
                    CustomerName = order.Customer.Name,
                    ItemCount = order.OrderItems.Count,
                    TotalAmount = order.OrderItems.Sum(item => item.Price * item.Quantity),
                    IsPaid = _dbContext.Payments.Any(payment => payment.OrderId == order.Id && payment.Status)
                })
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
