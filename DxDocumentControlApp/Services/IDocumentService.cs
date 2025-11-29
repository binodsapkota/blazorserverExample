using DxDocumentControlApp.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DxDocumentControlApp.Services
{
    public interface IDocumentService
    {
        Task<DocumentFile> UploadFileAsync(IFormFile file, Guid containerId, string uploadedBy);
        Task<Stream> DownloadFileAsync(Guid versionId);
        Task<IEnumerable<DocumentFile>> GetFilesAsync(Guid containerId);
        Task<IEnumerable<DocumentVersion>> GetFileHistoryAsync(Guid fileId);
        Task DeleteFileAsync(Guid fileId);
        Task<DocumentFile> ReplaceFileAsync(IFormFile file, Guid fileId, string uploadedBy);
    }
}
