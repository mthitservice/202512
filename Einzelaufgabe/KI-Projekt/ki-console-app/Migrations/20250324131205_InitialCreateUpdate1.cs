using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ki_console_app.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateUpdate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "firstame",
                table: "Records",
                newName: "firstname");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "firstname",
                table: "Records",
                newName: "firstame");
        }
    }
}
