using Microsoft.EntityFrameworkCore.Migrations;

namespace FirmwareServer.EntityLayer.Migrations
{
    public partial class ApplicationFirmwareId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FirmwareId",
                table: "Application",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirmwareId",
                table: "Application");
        }
    }
}
