using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Slask.Persistence.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tournaments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournaments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MiscBetCatalogue",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    TournamentId = table.Column<Guid>(nullable: false)
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
                name: "RoundBase",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PlayersPerGroupCount = table.Column<int>(nullable: false),
                    AdvancingPerGroupCount = table.Column<int>(nullable: false),
                    TournamentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoundBase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoundBase_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    TournamentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Settings_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Better",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: true),
                    TournamentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Better", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Better_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Better_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GroupBase",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    RoundId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupBase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupBase_RoundBase_RoundId",
                        column: x => x.RoundId,
                        principalTable: "RoundBase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerReference",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    TournamentId = table.Column<Guid>(nullable: false),
                    RoundBaseId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerReference", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerReference_RoundBase_RoundBaseId",
                        column: x => x.RoundBaseId,
                        principalTable: "RoundBase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlayerReference_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BetBase",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BetterId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BetBase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BetBase_Better_BetterId",
                        column: x => x.BetterId,
                        principalTable: "Better",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Match",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BestOf = table.Column<int>(nullable: false),
                    StartDateTime = table.Column<DateTime>(nullable: false),
                    GroupId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Match", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Match_GroupBase_GroupId",
                        column: x => x.GroupId,
                        principalTable: "GroupBase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PlayerReferenceId = table.Column<Guid>(nullable: true),
                    Score = table.Column<int>(nullable: false),
                    MatchId = table.Column<Guid>(nullable: false)
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
                    table.ForeignKey(
                        name: "FK_Player_PlayerReference_PlayerReferenceId",
                        column: x => x.PlayerReferenceId,
                        principalTable: "PlayerReference",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MiscBetPlayerEntry",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PlayerId = table.Column<Guid>(nullable: true),
                    Value = table.Column<int>(nullable: false),
                    MiscBetCatalogueId = table.Column<Guid>(nullable: true)
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
                name: "IX_BetBase_BetterId",
                table: "BetBase",
                column: "BetterId");

            migrationBuilder.CreateIndex(
                name: "IX_Better_TournamentId",
                table: "Better",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_Better_UserId",
                table: "Better",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupBase_RoundId",
                table: "GroupBase",
                column: "RoundId");

            migrationBuilder.CreateIndex(
                name: "IX_Match_GroupId",
                table: "Match",
                column: "GroupId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Player_MatchId",
                table: "Player",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Player_PlayerReferenceId",
                table: "Player",
                column: "PlayerReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerReference_RoundBaseId",
                table: "PlayerReference",
                column: "RoundBaseId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerReference_TournamentId",
                table: "PlayerReference",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_RoundBase_TournamentId",
                table: "RoundBase",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_Settings_TournamentId",
                table: "Settings",
                column: "TournamentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BetBase");

            migrationBuilder.DropTable(
                name: "MiscBetPlayerEntry");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "Better");

            migrationBuilder.DropTable(
                name: "MiscBetCatalogue");

            migrationBuilder.DropTable(
                name: "Player");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Match");

            migrationBuilder.DropTable(
                name: "PlayerReference");

            migrationBuilder.DropTable(
                name: "GroupBase");

            migrationBuilder.DropTable(
                name: "RoundBase");

            migrationBuilder.DropTable(
                name: "Tournaments");
        }
    }
}
