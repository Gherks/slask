using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Slask.Persistence.Migrations
{
    public partial class initial_clean_up : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BetBase_Better_BetterId",
                table: "BetBase");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupBase_RoundBase_RoundId",
                table: "GroupBase");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_GroupBase_GroupId",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerReference_RoundBase_RoundBaseId",
                table: "PlayerReference");

            migrationBuilder.DropForeignKey(
                name: "FK_RoundBase_Tournaments_TournamentId",
                table: "RoundBase");

            migrationBuilder.DropTable(
                name: "MiscBetPlayerEntry");

            migrationBuilder.DropTable(
                name: "MiscBetCatalogue");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoundBase",
                table: "RoundBase");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupBase",
                table: "GroupBase");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BetBase",
                table: "BetBase");

            migrationBuilder.RenameTable(
                name: "RoundBase",
                newName: "Round");

            migrationBuilder.RenameTable(
                name: "GroupBase",
                newName: "Group");

            migrationBuilder.RenameTable(
                name: "BetBase",
                newName: "Bet");

            migrationBuilder.RenameIndex(
                name: "IX_RoundBase_TournamentId",
                table: "Round",
                newName: "IX_Round_TournamentId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupBase_RoundId",
                table: "Group",
                newName: "IX_Group_RoundId");

            migrationBuilder.RenameIndex(
                name: "IX_BetBase_BetterId",
                table: "Bet",
                newName: "IX_Bet_BetterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Round",
                table: "Round",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Group",
                table: "Group",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bet",
                table: "Bet",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bet_Better_BetterId",
                table: "Bet",
                column: "BetterId",
                principalTable: "Better",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Group_Round_RoundId",
                table: "Group",
                column: "RoundId",
                principalTable: "Round",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Group_GroupId",
                table: "Match",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerReference_Round_RoundBaseId",
                table: "PlayerReference",
                column: "RoundBaseId",
                principalTable: "Round",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Round_Tournaments_TournamentId",
                table: "Round",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bet_Better_BetterId",
                table: "Bet");

            migrationBuilder.DropForeignKey(
                name: "FK_Group_Round_RoundId",
                table: "Group");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_Group_GroupId",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerReference_Round_RoundBaseId",
                table: "PlayerReference");

            migrationBuilder.DropForeignKey(
                name: "FK_Round_Tournaments_TournamentId",
                table: "Round");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Round",
                table: "Round");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Group",
                table: "Group");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bet",
                table: "Bet");

            migrationBuilder.RenameTable(
                name: "Round",
                newName: "RoundBase");

            migrationBuilder.RenameTable(
                name: "Group",
                newName: "GroupBase");

            migrationBuilder.RenameTable(
                name: "Bet",
                newName: "BetBase");

            migrationBuilder.RenameIndex(
                name: "IX_Round_TournamentId",
                table: "RoundBase",
                newName: "IX_RoundBase_TournamentId");

            migrationBuilder.RenameIndex(
                name: "IX_Group_RoundId",
                table: "GroupBase",
                newName: "IX_GroupBase_RoundId");

            migrationBuilder.RenameIndex(
                name: "IX_Bet_BetterId",
                table: "BetBase",
                newName: "IX_BetBase_BetterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoundBase",
                table: "RoundBase",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupBase",
                table: "GroupBase",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BetBase",
                table: "BetBase",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "MiscBetCatalogue",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TournamentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MiscBetCatalogue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MiscBetCatalogue_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MiscBetPlayerEntry",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MiscBetCatalogueId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MiscBetPlayerEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MiscBetPlayerEntry_MiscBetCatalogue_MiscBetCatalogueId",
                        column: x => x.MiscBetCatalogueId,
                        principalTable: "MiscBetCatalogue",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MiscBetPlayerEntry_Player_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Player",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MiscBetCatalogue_TournamentId",
                table: "MiscBetCatalogue",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_MiscBetPlayerEntry_MiscBetCatalogueId",
                table: "MiscBetPlayerEntry",
                column: "MiscBetCatalogueId");

            migrationBuilder.CreateIndex(
                name: "IX_MiscBetPlayerEntry_PlayerId",
                table: "MiscBetPlayerEntry",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_BetBase_Better_BetterId",
                table: "BetBase",
                column: "BetterId",
                principalTable: "Better",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupBase_RoundBase_RoundId",
                table: "GroupBase",
                column: "RoundId",
                principalTable: "RoundBase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_GroupBase_GroupId",
                table: "Match",
                column: "GroupId",
                principalTable: "GroupBase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerReference_RoundBase_RoundBaseId",
                table: "PlayerReference",
                column: "RoundBaseId",
                principalTable: "RoundBase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoundBase_Tournaments_TournamentId",
                table: "RoundBase",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
