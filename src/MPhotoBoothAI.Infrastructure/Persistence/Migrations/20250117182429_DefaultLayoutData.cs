using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MPhotoBoothAI.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DefaultLayoutData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OverlayImageDataEntity_LayoutDatas_LayoutDataEntityId",
                table: "OverlayImageDataEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_PhotoLayoutDataEntity_LayoutDatas_LayoutDataEntityId",
                table: "PhotoLayoutDataEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PhotoLayoutDataEntity",
                table: "PhotoLayoutDataEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OverlayImageDataEntity",
                table: "OverlayImageDataEntity");

            migrationBuilder.RenameTable(
                name: "PhotoLayoutDataEntity",
                newName: "PhotoLayouts");

            migrationBuilder.RenameTable(
                name: "OverlayImageDataEntity",
                newName: "OverlayImagesData");

            migrationBuilder.RenameIndex(
                name: "IX_PhotoLayoutDataEntity_LayoutDataEntityId",
                table: "PhotoLayouts",
                newName: "IX_PhotoLayouts_LayoutDataEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_OverlayImageDataEntity_LayoutDataEntityId",
                table: "OverlayImagesData",
                newName: "IX_OverlayImagesData_LayoutDataEntityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PhotoLayouts",
                table: "PhotoLayouts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OverlayImagesData",
                table: "OverlayImagesData",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "LayoutFormat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FormatWidth = table.Column<double>(type: "REAL", nullable: false),
                    FormatHeight = table.Column<double>(type: "REAL", nullable: false),
                    FormatRatio = table.Column<double>(type: "REAL", nullable: false),
                    SizeName = table.Column<string>(type: "NVARCHAR", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LayoutFormat", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "LayoutDatas",
                column: "Id",
                values: new object[]
                {
                    1,
                    2
                });

            migrationBuilder.InsertData(
                table: "LayoutFormat",
                columns: new[] { "Id", "FormatHeight", "FormatRatio", "FormatWidth", "SizeName" },
                values: new object[,]
                {
                    { 1, 1000.0, 0.33333333333333331, 3000.0, "5x15" },
                    { 2, 2000.0, 0.66666666666666663, 3000.0, "10x15" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_OverlayImagesData_LayoutDatas_LayoutDataEntityId",
                table: "OverlayImagesData",
                column: "LayoutDataEntityId",
                principalTable: "LayoutDatas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PhotoLayouts_LayoutDatas_LayoutDataEntityId",
                table: "PhotoLayouts",
                column: "LayoutDataEntityId",
                principalTable: "LayoutDatas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OverlayImagesData_LayoutDatas_LayoutDataEntityId",
                table: "OverlayImagesData");

            migrationBuilder.DropForeignKey(
                name: "FK_PhotoLayouts_LayoutDatas_LayoutDataEntityId",
                table: "PhotoLayouts");

            migrationBuilder.DropTable(
                name: "LayoutFormat");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PhotoLayouts",
                table: "PhotoLayouts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OverlayImagesData",
                table: "OverlayImagesData");

            migrationBuilder.DeleteData(
                table: "LayoutDatas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "LayoutDatas",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.RenameTable(
                name: "PhotoLayouts",
                newName: "PhotoLayoutDataEntity");

            migrationBuilder.RenameTable(
                name: "OverlayImagesData",
                newName: "OverlayImageDataEntity");

            migrationBuilder.RenameIndex(
                name: "IX_PhotoLayouts_LayoutDataEntityId",
                table: "PhotoLayoutDataEntity",
                newName: "IX_PhotoLayoutDataEntity_LayoutDataEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_OverlayImagesData_LayoutDataEntityId",
                table: "OverlayImageDataEntity",
                newName: "IX_OverlayImageDataEntity_LayoutDataEntityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PhotoLayoutDataEntity",
                table: "PhotoLayoutDataEntity",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OverlayImageDataEntity",
                table: "OverlayImageDataEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OverlayImageDataEntity_LayoutDatas_LayoutDataEntityId",
                table: "OverlayImageDataEntity",
                column: "LayoutDataEntityId",
                principalTable: "LayoutDatas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PhotoLayoutDataEntity_LayoutDatas_LayoutDataEntityId",
                table: "PhotoLayoutDataEntity",
                column: "LayoutDataEntityId",
                principalTable: "LayoutDatas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
