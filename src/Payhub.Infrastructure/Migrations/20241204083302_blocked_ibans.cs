using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Payhub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class blocked_ibans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BlacklistIbans",
                table: "BlacklistIbans");

            migrationBuilder.RenameTable(
                name: "BlacklistIbans",
                newName: "blacklist_ibans");

            migrationBuilder.RenameColumn(
                name: "Iban",
                table: "blacklist_ibans",
                newName: "iban");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "blacklist_ibans",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "blacklist_ibans",
                newName: "updated_date");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "blacklist_ibans",
                newName: "created_date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_date",
                table: "blacklist_ibans",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_date",
                table: "blacklist_ibans",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddPrimaryKey(
                name: "PK_blacklist_ibans",
                table: "blacklist_ibans",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_blacklist_ibans_iban",
                table: "blacklist_ibans",
                column: "iban",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_blacklist_ibans",
                table: "blacklist_ibans");

            migrationBuilder.DropIndex(
                name: "IX_blacklist_ibans_iban",
                table: "blacklist_ibans");

            migrationBuilder.RenameTable(
                name: "blacklist_ibans",
                newName: "BlacklistIbans");

            migrationBuilder.RenameColumn(
                name: "iban",
                table: "BlacklistIbans",
                newName: "Iban");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "BlacklistIbans",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_date",
                table: "BlacklistIbans",
                newName: "UpdatedDate");

            migrationBuilder.RenameColumn(
                name: "created_date",
                table: "BlacklistIbans",
                newName: "CreatedDate");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedDate",
                table: "BlacklistIbans",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "BlacklistIbans",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlacklistIbans",
                table: "BlacklistIbans",
                column: "Id");
        }
    }
}
