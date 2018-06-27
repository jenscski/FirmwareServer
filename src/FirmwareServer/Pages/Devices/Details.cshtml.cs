using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirmwareServer.Breadcrumb;
using FirmwareServer.EntityLayer;
using FirmwareServer.EntityLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FirmwareServer.Pages.Devices
{
    public class DetailsModel : PageModel, Breadcrumb.IBreadcrumbPage
    {
        private readonly Database _db;

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public Device Device { get; private set; }

        public DetailsModel(Database db)
        {
            _db = db;
        }

        public void OnGet()
        {
            Device = _db.Devices
                .Include(x => x.DeviceType)
                .Include(x => x.Firmware)
                .Include(x => x.CurrentFirmware)
                .First(x => x.Id == Id);
            if (Device == null)
            {
                throw new ApplicationException($"Unable to load device with ID '{Id}'.");
            }
        }

        IEnumerable<Breadcrumb.Breadcrumb> IBreadcrumbPage.Breadcrumbs()
        {
            return new[] { new Breadcrumb.Breadcrumb { Title = "Devices", Url = Url.Page("./Index") }, };
        }
    }
}