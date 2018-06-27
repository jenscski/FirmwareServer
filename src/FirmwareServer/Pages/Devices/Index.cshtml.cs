using FirmwareServer.Breadcrumb;
using FirmwareServer.EntityLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FirmwareServer.Pages.Devices
{
    public class IndexModel : PageModel, IBreadcrumbPage
    {
        private readonly Database _db;

        [TempData]
        public string StatusMessage { get; set; }

        public IEnumerable<RowModel> Devices { get; private set; }

        public class RowModel
        {
            public int Id { get; internal set; }

            public string Name { get; internal set; }

            public string DeviceType { get; internal set; }

            public int? CurrentFirmwareId { get; internal set; }

            public DateTimeOffset LastOnline { get; internal set; }

            public ChipType ChipType { get; internal set; }

            public int? FirmwareId { get; internal set; }
        }

        public IndexModel(Database db)
        {
            _db = db;
        }

        public void OnGet()
        {
            Devices = _db.Devices
                .GroupJoin(_db.DeviceTypes, d => d.DeviceTypeId, t => t.Id, (d, t) => new { Device = d, t })
                .SelectMany(x => x.t.DefaultIfEmpty(), (x, t) => new RowModel
                {
                    Id = x.Device.Id,
                    Name = x.Device.Name,
                    ChipType = x.Device.ChipType,
                    DeviceType = t.Name,
                    LastOnline = x.Device.LastOnline,

                    FirmwareId = x.Device.FirmwareId,
                    CurrentFirmwareId = x.Device.CurrentFirmwareId,
                })
                .OrderBy(x => x.Name.ToLower())
                .ToList();
        }

        IEnumerable<Breadcrumb.Breadcrumb> IBreadcrumbPage.Breadcrumbs()
        {
            return null;
        }
    }
}