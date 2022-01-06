using Microsoft.EntityFrameworkCore.Migrations;

namespace BookStoreAPI.Migrations
{
    public partial class QuantitySoldAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuantitySold",
                table: "Books",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantitySold",
                table: "Books");
        }
    }
}
