using Microsoft.EntityFrameworkCore;

namespace DxDocumentControlApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<DocumentContainer> DocumentContainers { get; set; }
        public DbSet<DocumentFile> DocumentFiles { get; set; }
        public DbSet<DocumentVersion> DocumentVersions { get; set; }
        public DbSet<DocumentAcl> DocumentAcls { get; set; }
        public DbSet<DocumentPolicy> DocumentPolicies { get; set; }
        public DbSet<DocumentAudit> DocumentAudits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Optional: Configure relationships & constraints
            modelBuilder.Entity<DocumentFile>()
                .HasMany(f => f.Versions)
                .WithOne(v => v.File)
                .HasForeignKey(v => v.FileId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DocumentContainer>()
                .HasMany(c => c.Files)
                .WithOne(f => f.Container)
                .HasForeignKey(f => f.ContainerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
