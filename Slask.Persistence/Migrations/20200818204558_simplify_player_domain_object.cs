using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Slask.Persistence.Migrations
{
    public partial class simplify_player_domain_object : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Player1Id",
                table: "Match",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Player2Id",
                table: "Match",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Match_Player1Id",
                table: "Match",
                column: "Player1Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Match_Player2Id",
                table: "Match",
                column: "Player2Id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Player_Player1Id",
                table: "Match",
                column: "Player1Id",
                principalTable: "Player",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Player_Player2Id",
                table: "Match",
                column: "Player2Id",
                principalTable: "Player",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Match_Player_Player1Id",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_Player_Player2Id",
                table: "Match");

            migrationBuilder.DropIndex(
                name: "IX_Match_Player1Id",
                table: "Match");

            migrationBuilder.DropIndex(
                name: "IX_Match_Player2Id",
                table: "Match");

            migrationBuilder.DropColumn(
                name: "Player1Id",
                table: "Match");

            migrationBuilder.DropColumn(
                name: "Player2Id",
                table: "Match");
        }
    }
}
