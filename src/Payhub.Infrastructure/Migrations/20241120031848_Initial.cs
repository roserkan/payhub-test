using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Payhub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "affiliates",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    IsDynamic = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_affiliates", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "banks",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    icon_url = table.Column<string>(type: "text", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_banks", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "blacklists",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    blacklist_type = table.Column<int>(type: "integer", nullable: false),
                    value = table.Column<string>(type: "text", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blacklists", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "infrastructures",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    address = table.Column<string>(type: "text", nullable: false),
                    deposit_address = table.Column<string>(type: "text", nullable: false),
                    withdraw_address = table.Column<string>(type: "text", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_infrastructures", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "payment_ways",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_ways", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    role_type = table.Column<int>(type: "integer", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "system_permissions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    key = table.Column<string>(type: "text", nullable: false),
                    permission_group = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_system_permissions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    username = table.Column<string>(type: "text", nullable: false),
                    password_hash = table.Column<byte[]>(type: "bytea", nullable: false),
                    password_salt = table.Column<byte[]>(type: "bytea", nullable: false),
                    is_two_factor_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    two_factor_secret = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sites",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    infrastructure_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    address = table.Column<string>(type: "text", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sites", x => x.id);
                    table.ForeignKey(
                        name: "FK_sites_infrastructures_infrastructure_id",
                        column: x => x.infrastructure_id,
                        principalTable: "infrastructures",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "accounts",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    account_number = table.Column<string>(type: "text", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    first_balance = table.Column<decimal>(type: "numeric", nullable: false),
                    password = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    email_password = table.Column<string>(type: "text", nullable: true),
                    email_imap_password = table.Column<string>(type: "text", nullable: true),
                    min_deposit_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    max_deposit_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    daily_deposit_amount_limit = table.Column<decimal>(type: "numeric", nullable: false),
                    daily_withdraw_amount_limit = table.Column<decimal>(type: "numeric", nullable: false),
                    payment_way_id = table.Column<int>(type: "integer", nullable: false),
                    affiliate_id = table.Column<int>(type: "integer", nullable: true),
                    bank_id = table.Column<int>(type: "integer", nullable: false),
                    account_type = table.Column<int>(type: "integer", nullable: false),
                    account_classification = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accounts", x => x.id);
                    table.ForeignKey(
                        name: "FK_accounts_affiliates_affiliate_id",
                        column: x => x.affiliate_id,
                        principalTable: "affiliates",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_accounts_banks_bank_id",
                        column: x => x.bank_id,
                        principalTable: "banks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_accounts_payment_ways_payment_way_id",
                        column: x => x.payment_way_id,
                        principalTable: "payment_ways",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "role_affiliate_permissions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    affiliate_id = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_affiliate_permissions", x => x.id);
                    table.ForeignKey(
                        name: "FK_role_affiliate_permissions_affiliates_affiliate_id",
                        column: x => x.affiliate_id,
                        principalTable: "affiliates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_role_affiliate_permissions_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "role_system_permissions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    system_permission_id = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_system_permissions", x => x.id);
                    table.ForeignKey(
                        name: "FK_role_system_permissions_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_role_system_permissions_system_permissions_system_permissio~",
                        column: x => x.system_permission_id,
                        principalTable: "system_permissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_roles", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_roles_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_roles_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    site_id = table.Column<int>(type: "integer", nullable: false),
                    site_customer_id = table.Column<string>(type: "text", nullable: true),
                    full_name = table.Column<string>(type: "text", nullable: true),
                    username = table.Column<string>(type: "text", nullable: true),
                    signup_date = table.Column<string>(type: "text", nullable: true),
                    identity_number = table.Column<string>(type: "text", nullable: true),
                    customer_ip_address = table.Column<string>(type: "text", nullable: true),
                    panel_customer_id = table.Column<string>(type: "text", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customers", x => x.id);
                    table.ForeignKey(
                        name: "FK_customers_sites_site_id",
                        column: x => x.site_id,
                        principalTable: "sites",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "role_site_permissions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    site_id = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_site_permissions", x => x.id);
                    table.ForeignKey(
                        name: "FK_role_site_permissions_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_role_site_permissions_sites_site_id",
                        column: x => x.site_id,
                        principalTable: "sites",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "site_payment_ways",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    site_id = table.Column<int>(type: "integer", nullable: false),
                    payment_way_id = table.Column<int>(type: "integer", nullable: false),
                    api_key = table.Column<string>(type: "text", nullable: false),
                    secret_key = table.Column<string>(type: "text", nullable: false),
                    commission = table.Column<decimal>(type: "numeric", nullable: false),
                    min_balance_limit = table.Column<decimal>(type: "numeric", nullable: false),
                    max_balance_limit = table.Column<decimal>(type: "numeric", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_site_payment_ways", x => x.id);
                    table.ForeignKey(
                        name: "FK_site_payment_ways_sites_site_id",
                        column: x => x.site_id,
                        principalTable: "sites",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "site_safe_moves",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    site_id = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_site_safe_moves", x => x.id);
                    table.ForeignKey(
                        name: "FK_site_safe_moves_sites_site_id",
                        column: x => x.site_id,
                        principalTable: "sites",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_site_safe_moves_users_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_site_safe_moves_users_UpdatedUserId",
                        column: x => x.UpdatedUserId,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "account_sites",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    account_id = table.Column<int>(type: "integer", nullable: false),
                    site_id = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_sites", x => x.id);
                    table.ForeignKey(
                        name: "FK_account_sites_accounts_account_id",
                        column: x => x.account_id,
                        principalTable: "accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_account_sites_sites_site_id",
                        column: x => x.site_id,
                        principalTable: "sites",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "deposits",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    site_id = table.Column<int>(type: "integer", nullable: false),
                    process_id = table.Column<string>(type: "text", nullable: false),
                    account_id = table.Column<int>(type: "integer", nullable: true),
                    panel_customer_id = table.Column<string>(type: "text", nullable: false),
                    site_customer_id = table.Column<string>(type: "text", nullable: false),
                    customer_full_name = table.Column<string>(type: "text", nullable: true),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    payed_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    redirect_url = table.Column<string>(type: "text", nullable: true),
                    payment_way_id = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    infra_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    payment_id = table.Column<string>(type: "text", nullable: false),
                    commission = table.Column<decimal>(type: "numeric", nullable: false),
                    transaction_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    dynamic_account_name = table.Column<string>(type: "text", nullable: true),
                    dynamic_account_number = table.Column<string>(type: "text", nullable: true),
                    created_user_id = table.Column<int>(type: "integer", nullable: true),
                    updated_user_id = table.Column<int>(type: "integer", nullable: true),
                    auto_updated_name = table.Column<string>(type: "text", nullable: true),
                    affiliate_id = table.Column<int>(type: "integer", nullable: true),
                    infra_callback_type = table.Column<int>(type: "integer", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposits", x => x.id);
                    table.ForeignKey(
                        name: "FK_deposits_accounts_account_id",
                        column: x => x.account_id,
                        principalTable: "accounts",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_deposits_payment_ways_payment_way_id",
                        column: x => x.payment_way_id,
                        principalTable: "payment_ways",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_deposits_sites_site_id",
                        column: x => x.site_id,
                        principalTable: "sites",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_deposits_users_created_user_id",
                        column: x => x.created_user_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_deposits_users_updated_user_id",
                        column: x => x.updated_user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "withdraws",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    site_id = table.Column<int>(type: "integer", nullable: false),
                    process_id = table.Column<string>(type: "text", nullable: false),
                    account_id = table.Column<int>(type: "integer", nullable: true),
                    panel_customer_id = table.Column<string>(type: "text", nullable: false),
                    site_customer_id = table.Column<string>(type: "text", nullable: false),
                    customer_full_name = table.Column<string>(type: "text", nullable: true),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    payed_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    payment_way_id = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    infra_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    transaction_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    customer_account_number = table.Column<string>(type: "text", nullable: false),
                    created_user_id = table.Column<int>(type: "integer", nullable: true),
                    updated_user_id = table.Column<int>(type: "integer", nullable: true),
                    auto_updated_name = table.Column<string>(type: "text", nullable: true),
                    affiliate_id = table.Column<int>(type: "integer", nullable: true),
                    infra_callback_type = table.Column<int>(type: "integer", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_withdraws", x => x.id);
                    table.ForeignKey(
                        name: "FK_withdraws_accounts_account_id",
                        column: x => x.account_id,
                        principalTable: "accounts",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_withdraws_payment_ways_payment_way_id",
                        column: x => x.payment_way_id,
                        principalTable: "payment_ways",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_withdraws_sites_site_id",
                        column: x => x.site_id,
                        principalTable: "sites",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_withdraws_users_created_user_id",
                        column: x => x.created_user_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_withdraws_users_updated_user_id",
                        column: x => x.updated_user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_account_sites_account_id",
                table: "account_sites",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_account_sites_site_id",
                table: "account_sites",
                column: "site_id");

            migrationBuilder.CreateIndex(
                name: "IX_accounts_account_number",
                table: "accounts",
                column: "account_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_accounts_affiliate_id",
                table: "accounts",
                column: "affiliate_id");

            migrationBuilder.CreateIndex(
                name: "IX_accounts_bank_id",
                table: "accounts",
                column: "bank_id");

            migrationBuilder.CreateIndex(
                name: "IX_accounts_payment_way_id",
                table: "accounts",
                column: "payment_way_id");

            migrationBuilder.CreateIndex(
                name: "IX_affiliates_name",
                table: "affiliates",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_banks_name",
                table: "banks",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_customers_panel_customer_id",
                table: "customers",
                column: "panel_customer_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_customers_site_id",
                table: "customers",
                column: "site_id");

            migrationBuilder.CreateIndex(
                name: "IX_deposits_account_id",
                table: "deposits",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_deposits_created_user_id",
                table: "deposits",
                column: "created_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_deposits_payment_way_id",
                table: "deposits",
                column: "payment_way_id");

            migrationBuilder.CreateIndex(
                name: "IX_deposits_process_id",
                table: "deposits",
                column: "process_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_deposits_site_id",
                table: "deposits",
                column: "site_id");

            migrationBuilder.CreateIndex(
                name: "IX_deposits_status_created_date",
                table: "deposits",
                columns: new[] { "status", "created_date" });

            migrationBuilder.CreateIndex(
                name: "IX_deposits_updated_user_id",
                table: "deposits",
                column: "updated_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_infrastructures_name",
                table: "infrastructures",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_payment_ways_name",
                table: "payment_ways",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_role_affiliate_permissions_affiliate_id",
                table: "role_affiliate_permissions",
                column: "affiliate_id");

            migrationBuilder.CreateIndex(
                name: "IX_role_affiliate_permissions_role_id",
                table: "role_affiliate_permissions",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_role_site_permissions_role_id",
                table: "role_site_permissions",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_role_site_permissions_site_id",
                table: "role_site_permissions",
                column: "site_id");

            migrationBuilder.CreateIndex(
                name: "IX_role_system_permissions_role_id",
                table: "role_system_permissions",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_role_system_permissions_system_permission_id",
                table: "role_system_permissions",
                column: "system_permission_id");

            migrationBuilder.CreateIndex(
                name: "IX_roles_name",
                table: "roles",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_site_payment_ways_api_key",
                table: "site_payment_ways",
                column: "api_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_site_payment_ways_payment_way_id",
                table: "site_payment_ways",
                column: "payment_way_id");

            migrationBuilder.CreateIndex(
                name: "IX_site_payment_ways_secret_key",
                table: "site_payment_ways",
                column: "secret_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_site_payment_ways_site_id",
                table: "site_payment_ways",
                column: "site_id");

            migrationBuilder.CreateIndex(
                name: "IX_site_safe_moves_CreatedUserId",
                table: "site_safe_moves",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_site_safe_moves_UpdatedUserId",
                table: "site_safe_moves",
                column: "UpdatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_site_safe_moves_site_id",
                table: "site_safe_moves",
                column: "site_id");

            migrationBuilder.CreateIndex(
                name: "IX_sites_infrastructure_id",
                table: "sites",
                column: "infrastructure_id");

            migrationBuilder.CreateIndex(
                name: "IX_sites_name",
                table: "sites",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_system_permissions_key",
                table: "system_permissions",
                column: "key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_roles_role_id",
                table: "user_roles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_roles_user_id",
                table: "user_roles",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_name",
                table: "users",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_username",
                table: "users",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_withdraws_account_id",
                table: "withdraws",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_withdraws_created_user_id",
                table: "withdraws",
                column: "created_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_withdraws_payment_way_id",
                table: "withdraws",
                column: "payment_way_id");

            migrationBuilder.CreateIndex(
                name: "IX_withdraws_process_id",
                table: "withdraws",
                column: "process_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_withdraws_site_id",
                table: "withdraws",
                column: "site_id");

            migrationBuilder.CreateIndex(
                name: "IX_withdraws_status_created_date",
                table: "withdraws",
                columns: new[] { "status", "created_date" });

            migrationBuilder.CreateIndex(
                name: "IX_withdraws_updated_user_id",
                table: "withdraws",
                column: "updated_user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "account_sites");

            migrationBuilder.DropTable(
                name: "blacklists");

            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropTable(
                name: "deposits");

            migrationBuilder.DropTable(
                name: "role_affiliate_permissions");

            migrationBuilder.DropTable(
                name: "role_site_permissions");

            migrationBuilder.DropTable(
                name: "role_system_permissions");

            migrationBuilder.DropTable(
                name: "site_payment_ways");

            migrationBuilder.DropTable(
                name: "site_safe_moves");

            migrationBuilder.DropTable(
                name: "user_roles");

            migrationBuilder.DropTable(
                name: "withdraws");

            migrationBuilder.DropTable(
                name: "system_permissions");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "accounts");

            migrationBuilder.DropTable(
                name: "sites");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "affiliates");

            migrationBuilder.DropTable(
                name: "banks");

            migrationBuilder.DropTable(
                name: "payment_ways");

            migrationBuilder.DropTable(
                name: "infrastructures");
        }
    }
}
