using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MPhotoBoothAI.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCameraSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CameraSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Iso = table.Column<string>(type: "NVARCHAR", maxLength: 20, nullable: false),
                    Aperture = table.Column<string>(type: "NVARCHAR", maxLength: 20, nullable: false),
                    ShutterSpeed = table.Column<string>(type: "NVARCHAR", maxLength: 20, nullable: false),
                    WhiteBalance = table.Column<string>(type: "NVARCHAR", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CameraSettings", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CameraSettings",
                columns: new[] { "Id", "Aperture", "Iso", "ShutterSpeed", "WhiteBalance" },
                values: new object[] { 1, "", "", "", "" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CameraSettings");
        }
    }
}
