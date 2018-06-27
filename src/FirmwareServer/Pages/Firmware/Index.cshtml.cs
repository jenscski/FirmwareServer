using System;
using System.Collections.Generic;
using System.Linq;
using FirmwareServer.EntityLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FirmwareServer.Pages.Firmware
{
    public class IndexModel : PageModel, Breadcrumb.IBreadcrumbPage
    {
        [TempData]
        public string StatusMessage { get; set; }

        public IEnumerable<RowModel> Firmware { get; private set; }

        public class RowModel
        {
            public int Id { get; set; }

            public DateTimeOffset Created { get; set; }

            public string Name { get; set; }

            public string Filename { get; set; }

            public bool IsActive { get; internal set; }

            public string DeviceType { get; internal set; }
        }

        public IActionResult OnGet([FromServices] Database db)
        {
            Firmware = db.Firmware
                .GroupJoin(db.DeviceTypes, dt => dt.DeviceTypeId, f => f.Id, (firmware, types) => new { firmware, types })
                .SelectMany(x => x.types.DefaultIfEmpty(), (x, type) => new RowModel
                {
                    Id = x.firmware.Id,
                    Name = x.firmware.Name,
                    DeviceType = type.Name,
                    Filename = x.firmware.Filename,
                    Created = x.firmware.Created,
                    IsActive = db.Devices.Any(y => y.CurrentFirmwareId == x.firmware.Id || y.FirmwareId == x.firmware.Id),
                })
                .OrderByDescending(x => x.Created)
                .ToList();

            return Page();
        }

        public IEnumerable<Breadcrumb.Breadcrumb> Breadcrumbs()
        {
            return null;
        }
    }
}