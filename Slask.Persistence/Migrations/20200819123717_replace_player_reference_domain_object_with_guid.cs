using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Slask.Persistence.Migrations
{
    public partial class replace_player_reference_domain_object_with_guid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Player_PlayerReference_PlayerReferenceId",
                table: "Player");

            migrationBuilder.DropIndex(
                name: "IX_Player_PlayerReferenceId",
                table: "Player");

            migrationBuilder.AlterColumn<Guid>(
                name: "PlayerReferenceId",
                table: "Player",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "PlayerReferenceId",
                table: "Player",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.CreateIndex(
                name: "IX_Player_PlayerReferenceId",
                table: "Player",
                column: "PlayerReferenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_PlayerReference_PlayerReferenceId",
                table: "Player",
                column: "PlayerReferenceId",
                principalTable: "PlayerReference",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
