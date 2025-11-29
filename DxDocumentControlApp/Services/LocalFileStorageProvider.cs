using System;
using System.IO;
using System.Threading.Tasks;

namespace DxDocumentControlApp.Services
{
    public class LocalFileStorageProvider : IFileStorageProvider
    {
        private readonly string _rootPath;

        public LocalFileStorageProvider(string rootPath)
        {
            _rootPath = rootPath;

            if (!Directory.Exists(_rootPath))
                Directory.CreateDirectory(_rootPath);
        }

        public async Task<string> SaveFileAsync(Stream fileStream, string fileName, string containerName)
        {
            string containerPath = Path.Combine(_rootPath, containerName);
            if (!Directory.Exists(containerPath))
                Directory.CreateDirectory(containerPath);

            string filePath = Path.Combine(containerPath, fileName);

            using (var file = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                await fileStream.CopyToAsync(file);
            }

            return filePath;
        }

        public async Task<Stream> GetFileAsync(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found", filePath);

            var memoryStream = new MemoryStream();
            using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                await file.CopyToAsync(memoryStream);
            }
            memoryStream.Position = 0;
            return memoryStream;
        }

        public async Task DeleteFileAsync(string filePath)
        {
            if (File.Exists(filePath))
                await Task.Run(() => File.Delete(filePath));
        }

        public bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}
