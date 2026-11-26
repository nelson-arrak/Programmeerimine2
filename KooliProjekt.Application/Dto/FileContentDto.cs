using System.IO;

namespace KooliProjekt.Application.Dto
{
    public class FileContentDto
    {
        public string FileName { get; set; }
        public Stream Content { get; set; }
    }
}
