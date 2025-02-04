using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MPhotoBoothAI.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class LayoutData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LayoutDatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LayoutDatas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OverlayImageDataEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Path = table.Column<string>(type: "NVARCHAR", maxLength: 100, nullable: false),
                    Top = table.Column<double>(type: "REAL", nullable: false),
                    Left = table.Column<double>(type: "REAL", nullable: false),
                    Angle = table.Column<double>(type: "REAL", nullable: false),
                    Scale = table.Column<double>(type: "REAL", nullable: false),
                    LayoutDataEntityId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OverlayImageDataEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OverlayImageDataEntity_LayoutDatas_LayoutDataEntityId",
                        column: x => x.LayoutDataEntityId,
                        principalTable: "LayoutDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhotoLayoutDataEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Top = table.Column<double>(type: "REAL", nullable: false),
                    Left = table.Column<double>(type: "REAL", nullable: false),
                    Angle = table.Column<double>(type: "REAL", nullable: false),
                    Scale = table.Column<double>(type: "REAL", nullable: false),
                    LayoutDataEntityId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotoLayoutDataEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhotoLayoutDataEntity_LayoutDatas_LayoutDataEntityId",
                        column: x => x.LayoutDataEntityId,
                        principalTable: "LayoutDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OverlayImageDataEntity_LayoutDataEntityId",
                table: "OverlayImageDataEntity",
                column: "LayoutDataEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_PhotoLayoutDataEntity_LayoutDataEntityId",
                table: "PhotoLayoutDataEntity",
                column: "LayoutDataEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OverlayImageDataEntity");

            migrationBuilder.DropTable(
                name: "PhotoLayoutDataEntity");

            migrationBuilder.DropTable(
                name: "LayoutDatas");
        }
    }
}
