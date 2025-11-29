using System.IO;
using System.Threading.Tasks;

namespace DxDocumentControlApp.Services
{
    public interface IFileStorageProvider
    {
        Task<string> SaveFileAsync(Stream fileStream, string fileName, string containerName);
        Task<Stream> GetFileAsync(string filePath);
        Task DeleteFileAsync(string filePath);
        bool FileExists(string filePath);
    }
}
