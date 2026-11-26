using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Payments
{
    public class GetPaymentQueryHandler : IRequestHandler<GetPaymentQuery, OperationResult<PaymentDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetPaymentQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PaymentDto>> Handle(GetPaymentQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PaymentDto>();
            result.Value = await _dbContext.Payments
                .AsNoTracking()
                .Where(payment => payment.Id == request.Id)
                .Select(payment => new PaymentDto
                {
                    Id = payment.Id,
                    OrderId = payment.OrderId,
                    OrderDate = payment.Order.OrderDate,
                    CustomerName = payment.Order.Customer.Name,
                    Amount = payment.Amount,
                    PaymentDate = payment.PaymentDate,
                    PaymentMethod = payment.PaymentMethod,
                    IsPaid = payment.Status
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
