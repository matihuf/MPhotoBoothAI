using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MPhotoBoothAI.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FaceSwapTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserSettings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "UserSettings",
                type: "DATETIME",
                nullable: false,
                defaultValueSql: "date('now')");

            migrationBuilder.CreateTable(
                name: "FaceSwapTemplateGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "NVARCHAR", maxLength: 50, nullable: false),
                    IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "date('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaceSwapTemplateGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FaceSwapTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Faces = table.Column<int>(type: "INTEGER", nullable: false),
                    FileName = table.Column<string>(type: "NVARCHAR", maxLength: 50, nullable: false),
                    FaceSwapTemplateGroupId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "date('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaceSwapTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FaceSwapTemplates_FaceSwapTemplateGroups_FaceSwapTemplateGroupId",
                        column: x => x.FaceSwapTemplateGroupId,
                        principalTable: "FaceSwapTemplateGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FaceSwapTemplates_FaceSwapTemplateGroupId",
                table: "FaceSwapTemplates",
                column: "FaceSwapTemplateGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FaceSwapTemplates");

            migrationBuilder.DropTable(
                name: "FaceSwapTemplateGroups");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "UserSettings");

            migrationBuilder.InsertData(
                table: "UserSettings",
                columns: new[] { "Id", "CultureInfoName" },
                values: new object[] { 1, "" });
        }
    }
}
