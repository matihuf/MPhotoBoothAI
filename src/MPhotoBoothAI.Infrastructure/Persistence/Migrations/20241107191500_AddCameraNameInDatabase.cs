using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MPhotoBoothAI.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCameraNameInDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserSettings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.CreateTable(
                name: "CameraSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Camera = table.Column<string>(type: "NVARCHAR", maxLength: 50, nullable: false),
                    Iso = table.Column<string>(type: "NVARCHAR", maxLength: 20, nullable: false),
                    Aperture = table.Column<string>(type: "NVARCHAR", maxLength: 20, nullable: false),
                    ShutterSpeed = table.Column<string>(type: "NVARCHAR", maxLength: 20, nullable: false),
                    WhiteBalance = table.Column<string>(type: "NVARCHAR", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CameraSettings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CameraSettings");

            migrationBuilder.InsertData(
                table: "UserSettings",
                columns: new[] { "Id", "CultureInfoName" },
                values: new object[] { 1, "" });
        }
    }
}
