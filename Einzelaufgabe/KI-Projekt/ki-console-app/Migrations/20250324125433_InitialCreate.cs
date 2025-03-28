using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ki_console_app.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Disciplins",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disciplins", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    disciplinid = table.Column<int>(type: "int", nullable: false),
                    elevation = table.Column<float>(type: "real", nullable: false),
                    event_day = table.Column<DateTime>(type: "datetime2", nullable: false),
                    longitude = table.Column<float>(type: "real", nullable: false),
                    latitude = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.id);
                    table.ForeignKey(
                        name: "FK_Events_Disciplins_disciplinid",
                        column: x => x.disciplinid,
                        principalTable: "Disciplins",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Records",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    firstame = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lastname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    record_day = table.Column<DateTime>(type: "datetime2", nullable: false),
                    sportseventid = table.Column<int>(type: "int", nullable: false),
                    club = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Distance = table.Column<float>(type: "real", nullable: false),
                    YearOfBirth = table.Column<int>(type: "int", nullable: false),
                    timeinSecond = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Records", x => x.id);
                    table.ForeignKey(
                        name: "FK_Records_Events_sportseventid",
                        column: x => x.sportseventid,
                        principalTable: "Events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_disciplinid",
                table: "Events",
                column: "disciplinid");

            migrationBuilder.CreateIndex(
                name: "IX_Records_sportseventid",
                table: "Records",
                column: "sportseventid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Records");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Disciplins");
        }
    }
}
