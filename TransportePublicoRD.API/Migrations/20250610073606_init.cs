using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportePublicoRD.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PublicRoutes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicRoutes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublicRouteId = table.Column<int>(type: "int", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    FrequencyMinutes = table.Column<int>(type: "int", nullable: false),
                    PublicRoutesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schedules_PublicRoutes_PublicRoutesId",
                        column: x => x.PublicRoutesId,
                        principalTable: "PublicRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stops",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    PublicRouteId = table.Column<int>(type: "int", nullable: false),
                    PublicRoutesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stops_PublicRoutes_PublicRoutesId",
                        column: x => x.PublicRoutesId,
                        principalTable: "PublicRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_PublicRoutesId",
                table: "Schedules",
                column: "PublicRoutesId");

            migrationBuilder.CreateIndex(
                name: "IX_Stops_PublicRoutesId",
                table: "Stops",
                column: "PublicRoutesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "Stops");

            migrationBuilder.DropTable(
                name: "PublicRoutes");
        }
    }
}
