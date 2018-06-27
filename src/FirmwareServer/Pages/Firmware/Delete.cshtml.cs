using FirmwareServer.Breadcrumb;
using FirmwareServer.EntityLayer;
using FirmwareServer.EntityLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FirmwareServer.Pages.Firmware
{
    public class DeleteModel : PageModel, Breadcrumb.IBreadcrumbPage
    {
        private readonly Database _db;

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public EntityLayer.Models.Firmware Firmware { get; private set; }

        public DeleteModel(Database db)
        {
            _db = db;
        }

        public IActionResult OnGet()
        {
            Firmware = _db.Firmware.Find(Id);
            if (Firmware == null)
            {
                throw new ApplicationException($"Unable to load firmware with ID '{Id}'.");
            }

            if (_db.Devices.Any(x => x.CurrentFirmwareId == Id))
            {
                StatusMessage = "Firmware is active, and cannot be deleted";
                return RedirectToPage("./Index");
            }

            return Page();
        }

        public IActionResult OnGetDelete()
        {
            Firmware = _db.Firmware.Find(Id);
            if (Firmware == null)
            {
                throw new ApplicationException($"Unable to load firmware with ID '{Id}'.");
            }

            if (_db.Devices.Any(x => x.CurrentFirmwareId == Id))
            {
                throw new ApplicationException("Unable to delete active firmware");
            }

            _db.Firmware.Remove(Firmware);
            _db.SaveChanges();

            StatusMessage = "Firmware has been deleted";
            return RedirectToPage("./Index");
        }

        public IEnumerable<Breadcrumb.Breadcrumb> Breadcrumbs()
        {
            return new[] { new Breadcrumb.Breadcrumb { Title = "Firmware", Url = Url.Page("./Index") } };
        }
    }
}