using FirmwareServer.EntityLayer;
using FirmwareServer.EntityLayer.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FirmwareServer.Pages
{
    public class IndexModel : PageModel
    {
        private Database _db;

        public IndexModel(EntityLayer.Database db)
        {
            _db = db;
        }

        public List<Device> Devices { get; private set; }
        public List<EntityLayer.Models.Firmware> Firmware { get; private set; }
        public List<DeviceLogModel> DeviceLog { get; private set; }

        public void OnGet()
        {
            Devices = _db.Devices
                .OrderByDescending(x => x.LastOnline)
                .Take(10)
                .ToList();

            Firmware = _db.Firmware
                .OrderByDescending(x => x.Created)
                .Take(10)
                .ToList();

            DeviceLog = _db.DeviceLog
                .GroupJoin(_db.Devices, l => l.DeviceId, d => d.Id, (l, d) => new { l, d })
                .SelectMany(x => x.d.DefaultIfEmpty(), (x, d) => new DeviceLogModel
                {
                    Created = x.l.Created,
                    Level = x.l.Level,
                    Message = x.l.Message,
                    Name = d.Name,
                })
                .OrderByDescending(x => x.Created)
                .Take(100)
                .ToList();
        }
        public class DeviceLogModel
        {
            public DateTimeOffset Created { get; internal set; }
            public LogLevel Level { get; internal set; }
            public string Message { get; internal set; }
            public string Name { get; internal set; }
        }
    }

}
