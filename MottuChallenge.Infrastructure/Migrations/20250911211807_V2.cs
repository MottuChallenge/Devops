using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace MottuChallenge.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "polygon_points");

            migrationBuilder.CreateTable(
                name: "sector_points",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    point_order = table.Column<int>(type: "int", nullable: false),
                    x = table.Column<double>(type: "double", nullable: false),
                    y = table.Column<double>(type: "double", nullable: false),
                    SectorId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sector_points", x => x.Id);
                    table.ForeignKey(
                        name: "FK_sector_points_sectors_SectorId",
                        column: x => x.SectorId,
                        principalTable: "sectors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "yard_points",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    point_order = table.Column<int>(type: "int", nullable: false),
                    x = table.Column<double>(type: "double", nullable: false),
                    y = table.Column<double>(type: "double", nullable: false),
                    YardId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_yard_points", x => x.Id);
                    table.ForeignKey(
                        name: "FK_yard_points_yards_YardId",
                        column: x => x.YardId,
                        principalTable: "yards",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_sector_points_SectorId",
                table: "sector_points",
                column: "SectorId");

            migrationBuilder.CreateIndex(
                name: "IX_yard_points_YardId",
                table: "yard_points",
                column: "YardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sector_points");

            migrationBuilder.DropTable(
                name: "yard_points");

            migrationBuilder.CreateTable(
                name: "polygon_points",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    sector_id = table.Column<Guid>(type: "char(36)", nullable: true),
                    yard_id = table.Column<Guid>(type: "char(36)", nullable: true),
                    point_order = table.Column<int>(type: "int", nullable: false),
                    x = table.Column<double>(type: "double", nullable: false),
                    y = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_polygon_points", x => x.id);
                    table.ForeignKey(
                        name: "FK_polygon_points_sectors_sector_id",
                        column: x => x.sector_id,
                        principalTable: "sectors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_polygon_points_yards_yard_id",
                        column: x => x.yard_id,
                        principalTable: "yards",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_polygon_points_sector_id",
                table: "polygon_points",
                column: "sector_id");

            migrationBuilder.CreateIndex(
                name: "IX_polygon_points_yard_id",
                table: "polygon_points",
                column: "yard_id");
        }
    }
}
