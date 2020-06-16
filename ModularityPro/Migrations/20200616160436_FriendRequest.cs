using Microsoft.EntityFrameworkCore.Migrations;

namespace ModularityPro.Migrations
{
    public partial class FriendRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Accepted",
                table: "Friends",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Responded",
                table: "Friends",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Accepted",
                table: "Friends");

            migrationBuilder.DropColumn(
                name: "Responded",
                table: "Friends");
        }
    }
}
