using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Payhub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class affiliate_commission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "affiliate_commission",
                table: "withdraws",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "affiliate_commission",
                table: "deposits",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "affiliate_commission",
                table: "withdraws");

            migrationBuilder.DropColumn(
                name: "affiliate_commission",
                table: "deposits");
        }
    }
}
