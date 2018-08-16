using FirmwareServer.Breadcrumb;
using FirmwareServer.EntityLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace FirmwareServer.Pages.Applications
{
    public class IndexModel : PageModel, IBreadcrumbPage
    {
        private readonly Database _db;

        [TempData]
        public string StatusMessage { get; set; }

        public IEnumerable<RowModel> Applications { get; private set; }

        public class RowModel
        {
            public int Id { get; internal set; }

            public string Name { get; internal set; }

            public string DeviceType { get; internal set; }

            public string Firmware { get; internal set; }

            public bool IsActive { get; internal set; }
        }

        public IndexModel(Database db)
        {
            _db = db;
        }

        public void OnGet()
        {
            Applications = _db.Applications
                .Select(x => new RowModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    DeviceType = x.DeviceType.Name,
                    Firmware = x.Firmware.Name,
                    IsActive = _db.Devices.Any(a => a.ApplicationId == x.Id),
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