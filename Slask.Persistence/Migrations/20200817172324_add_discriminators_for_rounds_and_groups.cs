using Microsoft.EntityFrameworkCore.Migrations;

namespace Slask.Persistence.Migrations
{
    public partial class add_discriminators_for_rounds_and_groups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Group");

            migrationBuilder.AddColumn<int>(
                name: "ContestType",
                table: "Group",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContestType",
                table: "Group");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Group",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
