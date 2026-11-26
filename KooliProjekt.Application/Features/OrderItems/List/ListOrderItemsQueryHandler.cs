using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.OrderItems
{
    public class ListOrderItemsQueryHandler : IRequestHandler<ListOrderItemsQuery, OperationResult<PagedResult<OrderItemListItemDto>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListOrderItemsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<OrderItemListItemDto>>> Handle(ListOrderItemsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<OrderItemListItemDto>>();
            result.Value = await _dbContext.OrderItems
                .AsNoTracking()
                .OrderByDescending(item => item.Order.Id)
                .ThenBy(item => item.Id)
                .Select(item => new OrderItemListItemDto
                {
                    Id = item.Id,
                    OrderId = item.Order.Id,
                    ProductName = item.Product.Name,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price,
                    LineTotal = item.Price * item.Quantity
                })
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
