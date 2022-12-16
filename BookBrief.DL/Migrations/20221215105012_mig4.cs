using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookBrief.DL.Migrations
{
    public partial class mig4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_TypeofBook__TypeofBookId",
                table: "Book");

            migrationBuilder.DropTable(
                name: "TypeofBook");

            migrationBuilder.DropIndex(
                name: "IX_Book__TypeofBookId",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "_TypeofBookId",
                table: "Book");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "Book",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "_TypeofBookId",
                table: "Book",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TypeofBook",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeofBook", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Book__TypeofBookId",
                table: "Book",
                column: "_TypeofBookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_TypeofBook__TypeofBookId",
                table: "Book",
                column: "_TypeofBookId",
                principalTable: "TypeofBook",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
