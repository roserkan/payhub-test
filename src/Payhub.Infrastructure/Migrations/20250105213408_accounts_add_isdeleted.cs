using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Payhub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class accounts_add_isdeleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "accounts",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "accounts");
        }
    }
}
