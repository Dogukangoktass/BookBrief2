using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookBrief.DL.Migrations
{
    public partial class CommentSharedControlAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsShared",
                table: "Comment",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsShared",
                table: "Comment");
        }
    }
}
