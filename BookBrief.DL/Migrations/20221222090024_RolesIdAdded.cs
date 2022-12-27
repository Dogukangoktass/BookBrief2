using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookBrief.DL.Migrations
{
    public partial class RolesIdAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "_RolesRoleId",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_User__RolesRoleId",
                table: "User",
                column: "_RolesRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Roles__RolesRoleId",
                table: "User",
                column: "_RolesRoleId",
                principalTable: "Roles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Roles__RolesRoleId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User__RolesRoleId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "_RolesRoleId",
                table: "User");
        }
    }
}
