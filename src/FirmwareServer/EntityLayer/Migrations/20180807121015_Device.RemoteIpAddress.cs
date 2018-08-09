using Microsoft.EntityFrameworkCore.Migrations;

namespace FirmwareServer.EntityLayer.Migrations
{
    public partial class DeviceRemoteIpAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RemoteIpAddress",
                table: "Device",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemoteIpAddress",
                table: "Device");
        }
    }
}
