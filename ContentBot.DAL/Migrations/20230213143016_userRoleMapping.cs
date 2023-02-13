using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContentBot.DAL.Migrations
{
    public partial class userRoleMapping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEnable",
                table: "AspNetRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEnable",
                table: "AspNetRoles");
        }
    }
}
