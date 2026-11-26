using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Categories
{
    public class DeleteCategoryCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}
