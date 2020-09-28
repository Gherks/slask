using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Slask.Persistence.Migrations
{
    public partial class rework_how_player_references_are_handled : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerReference_Round_RoundBaseId",
                table: "PlayerReference");

            migrationBuilder.DropIndex(
                name: "IX_PlayerReference_RoundBaseId",
                table: "PlayerReference");

            migrationBuilder.DropColumn(
                name: "RoundBaseId",
                table: "PlayerReference");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RoundBaseId",
                table: "PlayerReference",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerReference_RoundBaseId",
                table: "PlayerReference",
                column: "RoundBaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerReference_Round_RoundBaseId",
                table: "PlayerReference",
                column: "RoundBaseId",
                principalTable: "Round",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
