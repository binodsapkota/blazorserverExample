using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DxDocumentControlApp.Data
{
    public class DocumentContainer
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<DocumentFile> Files { get; set; } = new List<DocumentFile>();
    }

    public class DocumentFile
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FileName { get; set; }
        public string FileType { get; set; }
        public long Size { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("DocumentContainer")]
        public Guid ContainerId { get; set; }
        public DocumentContainer Container { get; set; }

        public ICollection<DocumentVersion> Versions { get; set; } = new List<DocumentVersion>();
    }

    public class DocumentVersion
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FilePath { get; set; }      // Path on disk or cloud
        public int VersionNumber { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        public string UploadedBy { get; set; }

        [ForeignKey("DocumentFile")]
        public Guid FileId { get; set; }
        public DocumentFile File { get; set; }
    }

    public class DocumentAcl
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Role { get; set; }
        public bool CanView { get; set; }
        public bool CanUpload { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanShare { get; set; }

        [ForeignKey("DocumentContainer")]
        public Guid ContainerId { get; set; }
        public DocumentContainer Container { get; set; }
    }

    public class DocumentPolicy
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string TenantId { get; set; }
        public long MaxFileSize { get; set; }
        public string AllowedExtensions { get; set; }  // CSV of allowed extensions
        public int MaxFilesPerContainer { get; set; }
    }

    public class DocumentAudit
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Action { get; set; }    // Upload, Download, Delete, Edit
        public string User { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Details { get; set; }

        [ForeignKey("DocumentFile")]
        public Guid? FileId { get; set; }
        public DocumentFile File { get; set; }
    }
}
