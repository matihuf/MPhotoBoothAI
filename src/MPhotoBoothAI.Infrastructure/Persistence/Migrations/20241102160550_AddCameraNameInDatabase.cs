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
                table: "CameraSettings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UserSettings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "Camera",
                table: "CameraSettings",
                type: "NVARCHAR",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Camera",
                table: "CameraSettings");

            migrationBuilder.InsertData(
                table: "CameraSettings",
                columns: new[] { "Id", "Aperture", "Iso", "ShutterSpeed", "WhiteBalance" },
                values: new object[] { 1, "", "", "", "" });

            migrationBuilder.InsertData(
                table: "UserSettings",
                columns: new[] { "Id", "CultureInfoName" },
                values: new object[] { 1, "" });
        }
    }
}
