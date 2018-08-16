using FirmwareServer.EntityLayer;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
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

        public List<DeviceLogModel> DeviceLog { get; private set; }

        public void OnGet()
        {
            DeviceLog = _db.DeviceLog
                .Include(x => x.Device)
                .Select(x => new DeviceLogModel
                {
                    Created = x.Created,
                    Level = x.Level,
                    Message = x.Message,
                    Name = x.Device.Name,
                })
                .OrderByDescending(x => x.Created)
                .Take(100)
                .ToList();
        }

        public IActionResult OnGetDevices()
        {
            var devices = _db.Devices
                .Include(x => x.DeviceType)
                .Include(x => x.Application)
                .OrderByDescending(x => x.LastOnline)
                .Take(10)
                .AsEnumerable()
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    chipType = x.ChipType.ToString(),
                    deviceType = x.DeviceType?.Name ?? string.Empty,
                    application = x.Application?.Name ?? string.Empty,
                    online = x.LastOnline.Humanize(),
                    details = Url.Page("./Devices/Details", new { id = x.Id }),
                });

            return new JsonResult(devices);
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
