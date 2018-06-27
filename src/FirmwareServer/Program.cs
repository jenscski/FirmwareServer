using FirmwareServer.EntityLayer;
using FirmwareServer.EntityLayer.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FirmwareServer
{
    public class Program
    {
        public static string ApplicationVersion { get; private set; }

        public static void Main(string[] args)
        {
            ApplicationVersion = typeof(Program).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var db = services.GetService<EntityLayer.Database>();

                db.Database.Migrate();

                SeedDeviceTypes(db);
            }

            host.Run();
        }

        private static void SeedDeviceTypes(Database db)
        {
            var list = new List<DeviceType>
            {
                new DeviceType { Id = 1, Active = true, ChipType = ChipType.ESP8266, Name = "Generic ESP8266 Module" },
                new DeviceType { Id = 2, Active = false, ChipType = ChipType.ESP8266, Name = "Generic ESP8285 Module" },
                new DeviceType { Id = 3, Active = false, ChipType = ChipType.ESP8266, Name = "ESPDuino (ESP-13 Module)" },
                new DeviceType { Id = 4, Active = false, ChipType = ChipType.ESP8266, Name = "Adafruit Feather HUZZAH ESP8266" },
                new DeviceType { Id = 5, Active = false, ChipType = ChipType.ESP8266, Name = "ESPresso Lite 1.0" },
                new DeviceType { Id = 6, Active = false, ChipType = ChipType.ESP8266, Name = "ESPresso Lite 2.0" },
                new DeviceType { Id = 7, Active = false, ChipType = ChipType.ESP8266, Name = "Phoenix 1.0" },
                new DeviceType { Id = 8, Active = false, ChipType = ChipType.ESP8266, Name = "Phoenix 2.0" },
                new DeviceType { Id = 9, Active = false, ChipType = ChipType.ESP8266, Name = "NodeMCU 0.9 (ESP-12 Module)" },
                new DeviceType { Id = 10, Active = true, ChipType = ChipType.ESP8266, Name = "NodeMCU 1.0 (ESP-12E Module)" },
                new DeviceType { Id = 11, Active = false, ChipType = ChipType.ESP8266, Name = "Olimex MOD-WIFI-ESP8266(-DEV)" },
                new DeviceType { Id = 12, Active = true, ChipType = ChipType.ESP8266, Name = "SparkFun ESP8266 Thing" },
                new DeviceType { Id = 13, Active = true, ChipType = ChipType.ESP8266, Name = "SparkFun ESP8266 Thing Dev" },
                new DeviceType { Id = 14, Active = false, ChipType = ChipType.ESP8266, Name = "SweetPea ESP-210" },
                new DeviceType { Id = 15, Active = false, ChipType = ChipType.ESP8266, Name = "WeMos D1 R2 & mini" },
                new DeviceType { Id = 16, Active = false, ChipType = ChipType.ESP8266, Name = "WeMos D1 mini Pro" },
                new DeviceType { Id = 17, Active = false, ChipType = ChipType.ESP8266, Name = "WeMos D1 mini Lite" },
                new DeviceType { Id = 18, Active = false, ChipType = ChipType.ESP8266, Name = "WeMos D1 R1" },
                new DeviceType { Id = 19, Active = false, ChipType = ChipType.ESP8266, Name = "ESPino (ESP-12 Module)" },
                new DeviceType { Id = 20, Active = false, ChipType = ChipType.ESP8266, Name = "ThaiEasyElec's ESPino" },
                new DeviceType { Id = 21, Active = false, ChipType = ChipType.ESP8266, Name = "WifInfo" },
                new DeviceType { Id = 22, Active = false, ChipType = ChipType.ESP8266, Name = "Arduino" },
                new DeviceType { Id = 23, Active = false, ChipType = ChipType.ESP8266, Name = "4D Systems gen4 IoD Range" },
                new DeviceType { Id = 24, Active = false, ChipType = ChipType.ESP8266, Name = "Digistump Oak" },
            };


            var existing = db.DeviceTypes.ToDictionary(x => x.Id);

            foreach (var item in list)
            {
                if (existing.TryGetValue(item.Id, out var tmp))
                {
                    tmp.Name = item.Name;
                }
                else
                {
                    db.DeviceTypes.Add(item);
                }
            }

            db.SaveChanges();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var runningInContainer = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build()
                .GetValue("DOTNET_RUNNING_IN_CONTAINER", false);

            var builder = WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    var data = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("FirmwareServer:AppData", runningInContainer ? "/var/lib/fwsrv" : "."),
                        new KeyValuePair<string, string>("FirmwareServer:IsRunningInContainer", runningInContainer.ToString())
                    };

                    config.AddInMemoryCollection(data);
                })
                .SuppressStatusMessages(runningInContainer)
                .UseStartup<Startup>();

            return builder;
        }
    }
}
