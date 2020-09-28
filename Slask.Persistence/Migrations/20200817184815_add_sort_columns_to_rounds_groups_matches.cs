using Microsoft.EntityFrameworkCore.Migrations;

namespace Slask.Persistence.Migrations
{
    public partial class add_sort_columns_to_rounds_groups_matches : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "Round",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "Match",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "Group",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "Round");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "Match");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "Group");
        }
    }
}
