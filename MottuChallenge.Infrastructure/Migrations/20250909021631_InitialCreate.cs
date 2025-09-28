using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace MottuChallenge.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "addresses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false),
                    street = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    number = table.Column<int>(type: "int", nullable: false),
                    neighborhood = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    city = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    state = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    zip_code = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: false),
                    country = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_addresses", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sector_types",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sector_types", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "yards",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false),
                    name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    address_id = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_yards", x => x.id);
                    table.ForeignKey(
                        name: "FK_yards_addresses_address_id",
                        column: x => x.address_id,
                        principalTable: "addresses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sectors",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false),
                    yard_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    sector_type_id = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sectors", x => x.id);
                    table.ForeignKey(
                        name: "FK_sectors_sector_types_sector_type_id",
                        column: x => x.sector_type_id,
                        principalTable: "sector_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sectors_yards_yard_id",
                        column: x => x.yard_id,
                        principalTable: "yards",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "polygon_points",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    yard_id = table.Column<Guid>(type: "char(36)", nullable: true),
                    sector_id = table.Column<Guid>(type: "char(36)", nullable: true),
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

            migrationBuilder.CreateTable(
                name: "spots",
                columns: table => new
                {
                    spot_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    sector_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    x = table.Column<double>(type: "double", nullable: false),
                    y = table.Column<double>(type: "double", nullable: false),
                    status = table.Column<string>(type: "longtext", nullable: false),
                    motorcycle_id = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_spots", x => x.spot_id);
                    table.ForeignKey(
                        name: "FK_spots_sectors_sector_id",
                        column: x => x.sector_id,
                        principalTable: "sectors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Motorcycles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Model = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    EngineType = table.Column<string>(type: "longtext", nullable: false),
                    Plate = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: false),
                    LastRevisionDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SpotId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Motorcycles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Motorcycles_spots_SpotId",
                        column: x => x.SpotId,
                        principalTable: "spots",
                        principalColumn: "spot_id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "logs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false),
                    message = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    motorcycle_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    previous_spot_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    destination_spot_id = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_logs", x => x.id);
                    table.ForeignKey(
                        name: "FK_logs_Motorcycles_motorcycle_id",
                        column: x => x.motorcycle_id,
                        principalTable: "Motorcycles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_logs_spots_destination_spot_id",
                        column: x => x.destination_spot_id,
                        principalTable: "spots",
                        principalColumn: "spot_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_logs_spots_previous_spot_id",
                        column: x => x.previous_spot_id,
                        principalTable: "spots",
                        principalColumn: "spot_id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_logs_destination_spot_id",
                table: "logs",
                column: "destination_spot_id");

            migrationBuilder.CreateIndex(
                name: "IX_logs_motorcycle_id",
                table: "logs",
                column: "motorcycle_id");

            migrationBuilder.CreateIndex(
                name: "IX_logs_previous_spot_id",
                table: "logs",
                column: "previous_spot_id");

            migrationBuilder.CreateIndex(
                name: "IX_Motorcycles_SpotId",
                table: "Motorcycles",
                column: "SpotId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_polygon_points_sector_id",
                table: "polygon_points",
                column: "sector_id");

            migrationBuilder.CreateIndex(
                name: "IX_polygon_points_yard_id",
                table: "polygon_points",
                column: "yard_id");

            migrationBuilder.CreateIndex(
                name: "IX_sectors_sector_type_id",
                table: "sectors",
                column: "sector_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_sectors_yard_id",
                table: "sectors",
                column: "yard_id");

            migrationBuilder.CreateIndex(
                name: "IX_spots_sector_id",
                table: "spots",
                column: "sector_id");

            migrationBuilder.CreateIndex(
                name: "IX_yards_address_id",
                table: "yards",
                column: "address_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "logs");

            migrationBuilder.DropTable(
                name: "polygon_points");

            migrationBuilder.DropTable(
                name: "Motorcycles");

            migrationBuilder.DropTable(
                name: "spots");

            migrationBuilder.DropTable(
                name: "sectors");

            migrationBuilder.DropTable(
                name: "sector_types");

            migrationBuilder.DropTable(
                name: "yards");

            migrationBuilder.DropTable(
                name: "addresses");
        }
    }
}
