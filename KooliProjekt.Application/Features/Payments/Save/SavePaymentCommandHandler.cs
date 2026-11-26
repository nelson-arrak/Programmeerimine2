using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Payments
{
    public class SavePaymentCommandHandler : IRequestHandler<SavePaymentCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SavePaymentCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SavePaymentCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var orderExists = await _dbContext.Orders
                .AnyAsync(order => order.Id == request.OrderId, cancellationToken);
            if (!orderExists)
            {
                return result.AddPropertyError(nameof(request.OrderId), "Cannot find order with ID " + request.OrderId);
            }

            var orderHasPayment = await _dbContext.Payments
                .AnyAsync(payment => payment.OrderId == request.OrderId && payment.Id != request.Id, cancellationToken);
            if (orderHasPayment)
            {
                return result.AddPropertyError(nameof(request.OrderId), "This order already has a payment");
            }

            var payment = new Payment();

            if (request.Id == 0)
            {
                _dbContext.Payments.Add(payment);
            }
            else
            {
                payment = await _dbContext.Payments.FindAsync(new object[] { request.Id }, cancellationToken);
                if (payment == null)
                {
                    return result.AddError("Cannot find payment with ID " + request.Id);
                }
            }

            payment.OrderId = request.OrderId;
            payment.Amount = request.Amount;
            payment.PaymentDate = request.PaymentDate;
            payment.PaymentMethod = request.PaymentMethod;
            payment.Status = request.IsPaid;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
