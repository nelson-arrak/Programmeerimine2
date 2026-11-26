using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Infrastructure.Files
{
    public interface IFileClient
    {
        Task Save(string fileName, string fileStore, Stream inputStream);
        Task<Stream> Get(string fileName, string fileStore);
        Task<bool> Exists(string fileName, string fileStore);
        Task Delete(string fileName, string fileStore);
        Task<IList<string>> List(string fileStore);
        Task DeleteStore(string fileStore);
    }
}
