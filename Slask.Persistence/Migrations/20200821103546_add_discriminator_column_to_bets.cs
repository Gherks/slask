using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Slask.Persistence.Migrations
{
    public partial class add_discriminator_column_to_bets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BetType",
                table: "Bet",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "MatchId",
                table: "Bet",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PlayerId",
                table: "Bet",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BetType",
                table: "Bet");

            migrationBuilder.DropColumn(
                name: "MatchId",
                table: "Bet");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "Bet");
        }
    }
}
