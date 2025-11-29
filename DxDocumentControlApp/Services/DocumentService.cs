using DxDocumentControlApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DxDocumentControlApp.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IFileStorageProvider _storageProvider;

        public DocumentService(ApplicationDbContext dbContext, IFileStorageProvider storageProvider)
        {
            _dbContext = dbContext;
            _storageProvider = storageProvider;
        }

        public async Task<DocumentFile> UploadFileAsync(IFormFile file, Guid containerId, string uploadedBy)
        {
            var container = await _dbContext.DocumentContainers.FindAsync(containerId);
            if (container == null) throw new Exception("Container not found");

            var docFile = new DocumentFile
            {
                FileName = file.FileName,
                FileType = Path.GetExtension(file.FileName),
                Size = file.Length,
                ContainerId = containerId
            };

            _dbContext.DocumentFiles.Add(docFile);
            await _dbContext.SaveChangesAsync();

            // Save version
            string filePath = await _storageProvider.SaveFileAsync(file.OpenReadStream(), file.FileName, containerId.ToString());
            var version = new DocumentVersion
            {
                FileId = docFile.Id,
                FilePath = filePath,
                VersionNumber = 1,
                UploadedBy = uploadedBy
            };
            _dbContext.DocumentVersions.Add(version);
            await _dbContext.SaveChangesAsync();

            return docFile;
        }

        public async Task<DocumentFile> ReplaceFileAsync(IFormFile file, Guid fileId, string uploadedBy)
        {
            var docFile = await _dbContext.DocumentFiles.Include(f => f.Versions).FirstOrDefaultAsync(f => f.Id == fileId);
            if (docFile == null) throw new Exception("File not found");

            int nextVersion = (docFile.Versions?.Count ?? 0) + 1;
            string filePath = await _storageProvider.SaveFileAsync(file.OpenReadStream(), file.FileName, docFile.ContainerId.ToString());

            var version = new DocumentVersion
            {
                FileId = docFile.Id,
                FilePath = filePath,
                VersionNumber = nextVersion,
                UploadedBy = uploadedBy
            };
            _dbContext.DocumentVersions.Add(version);
            await _dbContext.SaveChangesAsync();

            return docFile;
        }

        public async Task<Stream> DownloadFileAsync(Guid versionId)
        {
            var version = await _dbContext.DocumentVersions.FindAsync(versionId);
            if (version == null) throw new Exception("Version not found");

            return await _storageProvider.GetFileAsync(version.FilePath);
        }

        public async Task DeleteFileAsync(Guid fileId)
        {
            var file = await _dbContext.DocumentFiles.Include(f => f.Versions).FirstOrDefaultAsync(f => f.Id == fileId);
            if (file == null) return;

            foreach (var version in file.Versions)
            {
                await _storageProvider.DeleteFileAsync(version.FilePath);
            }

            _dbContext.DocumentFiles.Remove(file);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<DocumentFile>> GetFilesAsync(Guid containerId)
        {
            return await _dbContext.DocumentFiles
                .Where(f => f.ContainerId == containerId)
                .Include(f => f.Versions)
                .ToListAsync();
        }

        public async Task<IEnumerable<DocumentVersion>> GetFileHistoryAsync(Guid fileId)
        {
            return await _dbContext.DocumentVersions
                .Where(v => v.FileId == fileId)
                .OrderBy(v => v.VersionNumber)
                .ToListAsync();
        }
    }
}
