using System.IO;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Products
{
    // Tagastab salvestatud faili nime
    public class AddFileCommand : IRequest<OperationResult<string>>
    {
        public int ProductId { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public Stream InputStream { get; set; }
    }
}
