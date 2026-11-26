using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.OrderItems
{
    public class DeleteOrderItemCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}
