using FirmwareServer.EntityLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace FirmwareServer.EntityLayer
{
    public class Database : DbContext
    {
        public Database(DbContextOptions<Database> options)
            : base(options)
        {
        }

        public DbSet<Device> Devices { get; set; }

        public DbSet<Firmware> Firmware { get; set; }

        public DbSet<DeviceLog> DeviceLog { get; set; }

        public DbSet<DeviceType> DeviceTypes { get; set; }

        public DbSet<Application> Applications { get; set; }

        // Remember to update DatabaseServices.Backup()
    }

    public enum ChipType : int
    {
        ESP8266 = 0,
    }
}
