using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Orders
{
    public class DeleteOrderCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}
