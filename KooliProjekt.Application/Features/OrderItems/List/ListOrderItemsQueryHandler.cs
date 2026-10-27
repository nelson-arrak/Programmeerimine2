using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.OrderItems
{
    public class ListOrderItemsQueryHandler : IRequestHandler<ListOrderItemsQuery, IEnumerable<OrderItem>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListOrderItemsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<OrderItem>> Handle(ListOrderItemsQuery request, CancellationToken cancellationToken)
        {
            var result = await _dbContext.OrderItems.ToListAsync(cancellationToken);

            return result;
        }
    }
}
