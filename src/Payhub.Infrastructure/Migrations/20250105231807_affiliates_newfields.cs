using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Payhub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class affiliates_newfields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDynamic",
                table: "affiliates",
                newName: "is_dynamic");

            migrationBuilder.AddColumn<decimal>(
                name: "commission_rate",
                table: "affiliates",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "daily_deposit_limit",
                table: "affiliates",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "daily_withdraw_limit",
                table: "affiliates",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "deposit_limit_exceeded",
                table: "affiliates",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_deposit_active",
                table: "affiliates",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_withdraw_active",
                table: "affiliates",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "max_deposit_amount",
                table: "affiliates",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "max_withdraw_amount",
                table: "affiliates",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "min_deposit_amount",
                table: "affiliates",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "min_withdraw_amount",
                table: "affiliates",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "withdraw_limit_exceeded",
                table: "affiliates",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "AffiliateSites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AffiliateId = table.Column<int>(type: "integer", nullable: false),
                    SiteId = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AffiliateSites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AffiliateSites_affiliates_AffiliateId",
                        column: x => x.AffiliateId,
                        principalTable: "affiliates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AffiliateSites_sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "sites",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AffiliateSites_AffiliateId",
                table: "AffiliateSites",
                column: "AffiliateId");

            migrationBuilder.CreateIndex(
                name: "IX_AffiliateSites_SiteId",
                table: "AffiliateSites",
                column: "SiteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AffiliateSites");

            migrationBuilder.DropColumn(
                name: "commission_rate",
                table: "affiliates");

            migrationBuilder.DropColumn(
                name: "daily_deposit_limit",
                table: "affiliates");

            migrationBuilder.DropColumn(
                name: "daily_withdraw_limit",
                table: "affiliates");

            migrationBuilder.DropColumn(
                name: "deposit_limit_exceeded",
                table: "affiliates");

            migrationBuilder.DropColumn(
                name: "is_deposit_active",
                table: "affiliates");

            migrationBuilder.DropColumn(
                name: "is_withdraw_active",
                table: "affiliates");

            migrationBuilder.DropColumn(
                name: "max_deposit_amount",
                table: "affiliates");

            migrationBuilder.DropColumn(
                name: "max_withdraw_amount",
                table: "affiliates");

            migrationBuilder.DropColumn(
                name: "min_deposit_amount",
                table: "affiliates");

            migrationBuilder.DropColumn(
                name: "min_withdraw_amount",
                table: "affiliates");

            migrationBuilder.DropColumn(
                name: "withdraw_limit_exceeded",
                table: "affiliates");

            migrationBuilder.RenameColumn(
                name: "is_dynamic",
                table: "affiliates",
                newName: "IsDynamic");
        }
    }
}
