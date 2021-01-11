using Microsoft.EntityFrameworkCore.Migrations;

namespace AldeiaParental.Data.Migrations
{
    public partial class AddPersonalDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PersonalDocument",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentType = table.Column<string>(nullable: false),
                    DocumentNumber = table.Column<string>(nullable: false),
                    FilePath = table.Column<string>(nullable: true),
                    Valid = table.Column<bool>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalDocument_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonalDocument_UserId",
                table: "PersonalDocument",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonalDocument");
        }
    }
}
