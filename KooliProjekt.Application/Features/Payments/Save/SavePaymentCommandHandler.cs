using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Payments
{
    // Andmete kontroll (kohustuslikud väljad, olemasolu, unikaalsus) on SavePaymentCommandValidator klassis
    public class SavePaymentCommandHandler : IRequestHandler<SavePaymentCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SavePaymentCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SavePaymentCommand request, CancellationToken cancellationToken)
        {
            var entity = new Payment();

            if (request.Id == 0)
            {
                _dbContext.Payments.Add(entity);
            }
            else
            {
                entity = await _dbContext.Payments.FindAsync(new object[] { request.Id }, cancellationToken);
            }

            entity.OrderId = request.OrderId;
            entity.Amount = request.Amount;
            entity.PaymentDate = request.PaymentDate;
            entity.PaymentMethod = request.PaymentMethod;
            entity.Status = request.IsPaid;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new OperationResult();
        }
    }
}
