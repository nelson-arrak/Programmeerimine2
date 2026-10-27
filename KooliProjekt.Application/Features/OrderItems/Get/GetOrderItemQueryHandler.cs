using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using MediatR;

namespace KooliProjekt.Application.Features.OrderItems
{
    public class GetOrderItemQueryHandler : IRequestHandler<GetOrderItemQuery, OrderItem>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetOrderItemQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OrderItem> Handle(GetOrderItemQuery request, CancellationToken cancellationToken)
        {
            var result = await _dbContext.OrderItems.FindAsync(
                new object[] { request.Id },
                cancellationToken);

            return result;
        }
    }
}
