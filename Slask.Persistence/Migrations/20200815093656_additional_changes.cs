using Microsoft.EntityFrameworkCore.Migrations;

namespace Slask.Persistence.Migrations
{
    public partial class additional_changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ObjectState",
                table: "Tournaments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Group",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ObjectState",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Group");
        }
    }
}
