using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Infrastructure.Files
{
    public class LocalFileClient : IFileClient
    {
        private readonly string _filesRoot;

        public LocalFileClient(string filesRoot)
        {
            _filesRoot = Path.GetFullPath(filesRoot);
        }

        public async Task Save(string fileName, string fileStore, Stream inputStream)
        {
            var folderPath = GetFolderPath(fileStore);
            var filePath = GetFilePath(fileName, fileStore);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            using var fileStream = new FileStream(filePath, FileMode.Create);
            await inputStream.CopyToAsync(fileStream);
        }

        public Task<Stream> Get(string fileName, string fileStore)
        {
            var filePath = GetFilePath(fileName, fileStore);

            if (!File.Exists(filePath))
            {
                return Task.FromResult<Stream>(null);
            }

            Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

            return Task.FromResult(stream);
        }

        public Task<bool> Exists(string fileName, string fileStore)
        {
            return Task.FromResult(File.Exists(GetFilePath(fileName, fileStore)));
        }

        public Task Delete(string fileName, string fileStore)
        {
            var filePath = GetFilePath(fileName, fileStore);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            return Task.CompletedTask;
        }

        public Task<IList<string>> List(string fileStore)
        {
            var folderPath = GetFolderPath(fileStore);
            IList<string> files = new List<string>();

            if (Directory.Exists(folderPath))
            {
                files = Directory.GetFiles(folderPath)
                    .Select(Path.GetFileName)
                    .OrderBy(name => name)
                    .ToList();
            }

            return Task.FromResult(files);
        }

        public Task DeleteStore(string fileStore)
        {
            var folderPath = GetFolderPath(fileStore);

            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, true);
            }

            return Task.CompletedTask;
        }

        private string GetFolderPath(string fileStore)
        {
            var folderPath = Path.GetFullPath(Path.Combine(_filesRoot, fileStore));

            // Kaitse "../" tüüpi teekondade vastu - kaust peab jääma FilesRoot alla
            if (!folderPath.StartsWith(_filesRoot, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Invalid file store: " + fileStore);
            }

            return folderPath;
        }

        private string GetFilePath(string fileName, string fileStore)
        {
            if (string.IsNullOrWhiteSpace(fileName) || fileName != Path.GetFileName(fileName))
            {
                throw new ArgumentException("Invalid file name: " + fileName);
            }

            return Path.Combine(GetFolderPath(fileStore), fileName);
        }
    }
}
