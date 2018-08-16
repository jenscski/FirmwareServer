using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace FirmwareServer.EntityLayer.Migrations
{
    public partial class RemoveDeviceFirmwareId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Device",
                newName: "DeviceOld");

            migrationBuilder.CreateTable(
                name: "Device",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DeviceTypeId = table.Column<int>(nullable: true),
                    Created = table.Column<DateTimeOffset>(nullable: false),
                    LastOnline = table.Column<DateTimeOffset>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    StaMac = table.Column<string>(nullable: true),
                    ApMac = table.Column<string>(nullable: true),
                    SdkVersion = table.Column<string>(nullable: true),
                    Version = table.Column<string>(nullable: true),
                    CurrentFirmwareId = table.Column<int>(nullable: true),
                    FreeSpace = table.Column<int>(nullable: true),
                    ChipSize = table.Column<int>(nullable: true),
                    ChipType = table.Column<int>(nullable: false),
                    RemoteIpAddress = table.Column<string>(nullable: true),
                    ApplicationId = table.Column<int>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Device", x => x.Id);
                });

            migrationBuilder.Sql("INSERT INTO Device SELECT Id, DeviceTypeId, Created, LastOnline, Name, StaMac, ApMac, SdkVersion, Version, CurrentFirmwareId, FreeSpace, ChipSize, ChipType, RemoteIpAddress, ApplicationId FROM DeviceOld");

            migrationBuilder.DropTable(name: "DeviceOld");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
