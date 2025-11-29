using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DxDocumentControlApp.Migrations
{
    /// <inheritdoc />
    public partial class initialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentContainers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentContainers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentPolicies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxFileSize = table.Column<long>(type: "bigint", nullable: false),
                    AllowedExtensions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxFilesPerContainer = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentPolicies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentAcls",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CanView = table.Column<bool>(type: "bit", nullable: false),
                    CanUpload = table.Column<bool>(type: "bit", nullable: false),
                    CanEdit = table.Column<bool>(type: "bit", nullable: false),
                    CanDelete = table.Column<bool>(type: "bit", nullable: false),
                    CanShare = table.Column<bool>(type: "bit", nullable: false),
                    ContainerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentAcls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentAcls_DocumentContainers_ContainerId",
                        column: x => x.ContainerId,
                        principalTable: "DocumentContainers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContainerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentFiles_DocumentContainers_ContainerId",
                        column: x => x.ContainerId,
                        principalTable: "DocumentContainers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentAudits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    User = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentAudits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentAudits_DocumentFiles_FileId",
                        column: x => x.FileId,
                        principalTable: "DocumentFiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DocumentVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VersionNumber = table.Column<int>(type: "int", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UploadedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentVersions_DocumentFiles_FileId",
                        column: x => x.FileId,
                        principalTable: "DocumentFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentAcls_ContainerId",
                table: "DocumentAcls",
                column: "ContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentAudits_FileId",
                table: "DocumentAudits",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentFiles_ContainerId",
                table: "DocumentFiles",
                column: "ContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentVersions_FileId",
                table: "DocumentVersions",
                column: "FileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentAcls");

            migrationBuilder.DropTable(
                name: "DocumentAudits");

            migrationBuilder.DropTable(
                name: "DocumentPolicies");

            migrationBuilder.DropTable(
                name: "DocumentVersions");

            migrationBuilder.DropTable(
                name: "DocumentFiles");

            migrationBuilder.DropTable(
                name: "DocumentContainers");
        }
    }
}
