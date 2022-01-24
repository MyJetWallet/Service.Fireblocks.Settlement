using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Service.Fireblocks.Settlement.Postgres.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "fireblocks-settlements");

            migrationBuilder.CreateTable(
                name: "transfers",
                schema: "fireblocks-settlements",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DestinationVaultAccountId = table.Column<string>(type: "text", nullable: true),
                    AsssetSymbol = table.Column<string>(type: "text", nullable: true),
                    AsssetNetwork = table.Column<string>(type: "text", nullable: true),
                    FireblocksAssetId = table.Column<string>(type: "text", nullable: true),
                    AccountsInTransfers = table.Column<int>(type: "integer", nullable: false),
                    Threshold = table.Column<decimal>(type: "numeric", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    StartedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CompletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transfers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_transfers_AsssetSymbol_AsssetNetwork_Status",
                schema: "fireblocks-settlements",
                table: "transfers",
                columns: new[] { "AsssetSymbol", "AsssetNetwork", "Status" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "transfers",
                schema: "fireblocks-settlements");
        }
    }
}
