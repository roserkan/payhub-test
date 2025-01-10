using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Payhub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class affiliate_safe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "affiliate_safe_moves",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    affiliate_id = table.Column<int>(type: "integer", nullable: false),
                    transaction_type = table.Column<int>(type: "integer", nullable: false),
                    move_type = table.Column<int>(type: "integer", nullable: false),
                    amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    commission_rate = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    commission_amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    transaction_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedUserId = table.Column<int>(type: "integer", nullable: true),
                    UpdatedUserId = table.Column<int>(type: "integer", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_affiliate_safe_moves", x => x.id);
                    table.ForeignKey(
                        name: "FK_affiliate_safe_moves_affiliates_affiliate_id",
                        column: x => x.affiliate_id,
                        principalTable: "affiliates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_affiliate_safe_moves_users_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_affiliate_safe_moves_users_UpdatedUserId",
                        column: x => x.UpdatedUserId,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_withdraws_affiliate_id",
                table: "withdraws",
                column: "affiliate_id");

            migrationBuilder.CreateIndex(
                name: "IX_deposits_affiliate_id",
                table: "deposits",
                column: "affiliate_id");

            migrationBuilder.CreateIndex(
                name: "IX_affiliate_safe_moves_affiliate_id",
                table: "affiliate_safe_moves",
                column: "affiliate_id");

            migrationBuilder.CreateIndex(
                name: "IX_affiliate_safe_moves_CreatedUserId",
                table: "affiliate_safe_moves",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_affiliate_safe_moves_UpdatedUserId",
                table: "affiliate_safe_moves",
                column: "UpdatedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_deposits_affiliates_affiliate_id",
                table: "deposits",
                column: "affiliate_id",
                principalTable: "affiliates",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_withdraws_affiliates_affiliate_id",
                table: "withdraws",
                column: "affiliate_id",
                principalTable: "affiliates",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_deposits_affiliates_affiliate_id",
                table: "deposits");

            migrationBuilder.DropForeignKey(
                name: "FK_withdraws_affiliates_affiliate_id",
                table: "withdraws");

            migrationBuilder.DropTable(
                name: "affiliate_safe_moves");

            migrationBuilder.DropIndex(
                name: "IX_withdraws_affiliate_id",
                table: "withdraws");

            migrationBuilder.DropIndex(
                name: "IX_deposits_affiliate_id",
                table: "deposits");
        }
    }
}
