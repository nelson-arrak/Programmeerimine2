using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Payments
{
    public class ListPaymentsQueryHandler : IRequestHandler<ListPaymentsQuery, OperationResult<PagedResult<PaymentListItemDto>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListPaymentsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<PaymentListItemDto>>> Handle(ListPaymentsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<PaymentListItemDto>>();
            result.Value = await _dbContext.Payments
                .AsNoTracking()
                .OrderByDescending(payment => payment.PaymentDate)
                .ThenByDescending(payment => payment.Id)
                .Select(payment => new PaymentListItemDto
                {
                    Id = payment.Id,
                    OrderId = payment.OrderId,
                    CustomerName = payment.Order.Customer.Name,
                    Amount = payment.Amount,
                    PaymentDate = payment.PaymentDate,
                    PaymentMethod = payment.PaymentMethod,
                    IsPaid = payment.Status
                })
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
