using FirmwareServer.Breadcrumb;
using FirmwareServer.EntityLayer;
using FirmwareServer.EntityLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FirmwareServer.Pages.Devices
{
    public class DeleteModel : PageModel, IBreadcrumbPage
    {
        private readonly Database _db;

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public DeleteModel(Database db)
        {
            _db = db;
        }

        public Device Device { get; private set; }

        public void OnGet()
        {
            Device = _db.Devices.Find(Id);
            if (Device == null)
            {
                throw new ApplicationException($"Unable to load device with ID '{Id}'.");
            }
        }

        public IActionResult OnGetDelete()
        {
            Device = _db.Devices.Find(Id);
            if (Device == null)
            {
                throw new ApplicationException($"Unable to load firmware with ID '{Id}'.");
            }

            _db.DeviceLog.RemoveRange(_db.DeviceLog.Where(x => x.DeviceId == Id));
            _db.Devices.Remove(Device);
            _db.SaveChanges();

            StatusMessage = "Device has been deleted";
            return RedirectToPage("./Index");
        }

        IEnumerable<Breadcrumb.Breadcrumb> IBreadcrumbPage.Breadcrumbs()
        {
            return new[] { new Breadcrumb.Breadcrumb { Title = "Devices", Url = Url.Page("./Index") }, };
        }
    }
}