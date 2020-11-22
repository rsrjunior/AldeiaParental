using Microsoft.EntityFrameworkCore.Migrations;

namespace AldeiaParental.Data.Migrations
{
    public partial class ServiceLocationsAndRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Region",
                newName: "Id");

            migrationBuilder.CreateTable(
                name: "ServiceLocation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AtCustomerHome = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    UserId1 = table.Column<string>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    RegionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceLocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceLocation_Region_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Region",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceLocation_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceLocation_RegionId",
                table: "ServiceLocation",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceLocation_UserId1",
                table: "ServiceLocation",
                column: "UserId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceLocation");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Region",
                newName: "ID");
        }
    }
}
