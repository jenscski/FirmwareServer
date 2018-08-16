using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace FirmwareServer.EntityLayer.Migrations
{
    public partial class FirmwareApplicationId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Firmware",
                newName: "FirmwareOld");

            migrationBuilder.CreateTable(
               name: "Firmware",
               columns: table => new
               {
                   Id = table.Column<int>(nullable: false)
                       .Annotation("Sqlite:Autoincrement", true),
                   Created = table.Column<DateTimeOffset>(nullable: false),
                   ApplicationId = table.Column<int>(nullable: false),
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

            migrationBuilder.Sql("INSERT INTO Firmware SELECT Id, Created, 0, Name, Description, Filename, Data, MD5 FROM FirmwareOld");

            migrationBuilder.DropTable(name: "FirmwareOld");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
