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
                    DocumentType = table.Column<string>(nullable: true),
                    DocumentNumber = table.Column<string>(nullable: true),
                    FilePath = table.Column<string>(nullable: true),
                    Valid = table.Column<bool>(nullable: true),
                    AldeiaParentalUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalDocument_AspNetUsers_AldeiaParentalUserId",
                        column: x => x.AldeiaParentalUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonalDocument_AldeiaParentalUserId",
                table: "PersonalDocument",
                column: "AldeiaParentalUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonalDocument");
        }
    }
}
