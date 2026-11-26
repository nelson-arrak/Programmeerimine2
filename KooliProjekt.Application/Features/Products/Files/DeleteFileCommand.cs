using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Products
{
    public class DeleteFileCommand : IRequest<OperationResult>
    {
        public int ProductId { get; set; }
        public string FileName { get; set; }
    }
}
