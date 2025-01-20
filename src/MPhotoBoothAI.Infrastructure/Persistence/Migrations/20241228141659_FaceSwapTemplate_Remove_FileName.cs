using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MPhotoBoothAI.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FaceSwapTemplate_Remove_FileName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "FaceSwapTemplates");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "FaceSwapTemplates",
                type: "NVARCHAR",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
