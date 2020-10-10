using Microsoft.EntityFrameworkCore.Migrations;

namespace AldeiaParental.Data.Migrations
{
    public partial class ServiceLocationsUserIdString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceLocation_AspNetUsers_UserId1",
                table: "ServiceLocation");

            migrationBuilder.DropIndex(
                name: "IX_ServiceLocation_UserId1",
                table: "ServiceLocation");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "ServiceLocation");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ServiceLocation",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceLocation_UserId",
                table: "ServiceLocation",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceLocation_AspNetUsers_UserId",
                table: "ServiceLocation",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceLocation_AspNetUsers_UserId",
                table: "ServiceLocation");

            migrationBuilder.DropIndex(
                name: "IX_ServiceLocation_UserId",
                table: "ServiceLocation");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "ServiceLocation",
                type: "int",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "ServiceLocation",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceLocation_UserId1",
                table: "ServiceLocation",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceLocation_AspNetUsers_UserId1",
                table: "ServiceLocation",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
