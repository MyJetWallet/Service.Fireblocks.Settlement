using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Service.Fireblocks.Settlement.Postgres.Migrations
{
    public partial class TotalBalanceAndUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalBalance",
                schema: "fireblocks-settlements",
                table: "transfers",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "fireblocks-settlements",
                table: "transfers",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalBalance",
                schema: "fireblocks-settlements",
                table: "transfers");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "fireblocks-settlements",
                table: "transfers");
        }
    }
}
