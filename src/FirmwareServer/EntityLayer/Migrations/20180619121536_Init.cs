using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FirmwareServer.EntityLayer.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    FirmwareId = table.Column<int>(nullable: true),
                    CurrentFirmwareId = table.Column<int>(nullable: true),
                    FreeSpace = table.Column<int>(nullable: true),
                    ChipSize = table.Column<int>(nullable: true),
                    ChipType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Device", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<DateTimeOffset>(nullable: false),
                    DeviceId = table.Column<int>(nullable: true),
                    Level = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    ChipType = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Firmware",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<DateTimeOffset>(nullable: false),
                    DeviceTypeId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Filename = table.Column<string>(nullable: true),
                    Data = table.Column<byte[]>(nullable: true),
                    MD5 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Firmware", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Device");

            migrationBuilder.DropTable(
                name: "DeviceLog");

            migrationBuilder.DropTable(
                name: "DeviceType");

            migrationBuilder.DropTable(
                name: "Firmware");
        }
    }
}
