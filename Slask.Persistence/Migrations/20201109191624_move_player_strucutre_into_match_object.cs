using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Slask.Persistence.Migrations
{
    public partial class move_player_strucutre_into_match_object : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Match_Player_Player1Id",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_Player_Player2Id",
                table: "Match");

            migrationBuilder.DropTable(
                name: "Player");

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

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "Bet");

            migrationBuilder.AddColumn<int>(
                name: "Player1Score",
                table: "Match",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Player2Score",
                table: "Match",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "PlayerReference1Id",
                table: "Match",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PlayerReference2Id",
                table: "Match",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PlayerReferenceId",
                table: "Bet",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Player1Score",
                table: "Match");

            migrationBuilder.DropColumn(
                name: "Player2Score",
                table: "Match");

            migrationBuilder.DropColumn(
                name: "PlayerReference1Id",
                table: "Match");

            migrationBuilder.DropColumn(
                name: "PlayerReference2Id",
                table: "Match");

            migrationBuilder.DropColumn(
                name: "PlayerReferenceId",
                table: "Bet");

            migrationBuilder.AddColumn<Guid>(
                name: "Player1Id",
                table: "Match",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Player2Id",
                table: "Match",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PlayerId",
                table: "Bet",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlayerReferenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Player_Match_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Match",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Player_MatchId",
                table: "Player",
                column: "MatchId");

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
    }
}
