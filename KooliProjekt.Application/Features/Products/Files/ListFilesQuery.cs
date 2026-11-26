using System.Collections.Generic;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Products
{
    public class ListFilesQuery : IRequest<OperationResult<IList<string>>>
    {
        public int ProductId { get; set; }
    }
}
