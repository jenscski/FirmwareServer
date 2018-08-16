using Microsoft.EntityFrameworkCore.Migrations;

namespace FirmwareServer.EntityLayer.Migrations
{
    public partial class DeviceApplicationId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApplicationId",
                table: "Device",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "Device");
        }
    }
}
