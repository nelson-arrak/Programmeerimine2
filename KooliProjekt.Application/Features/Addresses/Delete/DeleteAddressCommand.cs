using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Addresses
{
    public class DeleteAddressCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}
