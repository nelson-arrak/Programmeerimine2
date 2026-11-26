using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.OrderItems
{
    public class GetOrderItemQuery : IRequest<OperationResult<OrderItemDto>>
    {
        public int Id { get; set; }
    }
}
