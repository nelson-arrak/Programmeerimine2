using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Products
{
    public class GetFileCommand : IRequest<OperationResult<FileContentDto>>
    {
        public int ProductId { get; set; }
        public string FileName { get; set; }
    }
}
