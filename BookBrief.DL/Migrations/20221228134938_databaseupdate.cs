using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookBrief.DL.Migrations
{
    public partial class databaseupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_User__UserId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment__UserId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "_UserId",
                table: "Comment");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "_UserId",
                table: "Comment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Comment__UserId",
                table: "Comment",
                column: "_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_User__UserId",
                table: "Comment",
                column: "_UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
