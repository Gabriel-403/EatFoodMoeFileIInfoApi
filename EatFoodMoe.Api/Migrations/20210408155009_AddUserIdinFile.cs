using Microsoft.EntityFrameworkCore.Migrations;

namespace EatFoodMoe.Api.Migrations
{
    public partial class AddUserIdinFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "EatFoodFiles",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "EatFoodFiles");
        }
    }
}
