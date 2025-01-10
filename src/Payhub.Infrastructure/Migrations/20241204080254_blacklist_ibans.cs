using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Payhub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class blacklist_ibans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_iban_blacklisted",
                table: "withdraws",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_iban_blacklisted",
                table: "withdraws");
        }
    }
}
