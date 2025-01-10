using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Payhub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class botmovefixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TryCount",
                table: "havale_bot_moves",
                newName: "status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "status",
                table: "havale_bot_moves",
                newName: "TryCount");
        }
    }
}
