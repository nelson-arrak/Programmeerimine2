using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Products
{
    public class DeleteProductCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}
