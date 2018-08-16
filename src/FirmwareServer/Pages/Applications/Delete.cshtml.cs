using FirmwareServer.Breadcrumb;
using FirmwareServer.EntityLayer;
using FirmwareServer.EntityLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FirmwareServer.Pages.Applications
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

        public Application Application { get; private set; }

        public void OnGet()
        {
            Application = _db.Applications
                .Include(x => x.Firmware)
                .Include(x => x.DeviceType)
                .FirstOrDefault(x => x.Id == Id);
            if (Application == null)
            {
                throw new ApplicationException($"Unable to load application with ID '{Id}'.");
            }
        }

        public IActionResult OnGetDelete()
        {
            Application = _db.Applications.Find(Id);
            if (Application == null)
            {
                throw new ApplicationException($"Unable to load application with ID '{Id}'.");
            }

            if (_db.Devices.Any(a => a.ApplicationId == Id))
            {
                throw new ApplicationException("Unable to delete active application");
            }

            _db.Firmware.RemoveRange(_db.Firmware.Where(x => x.ApplicationId == Id).AsNoTracking());
            _db.SaveChanges();

            _db.Applications.Remove(Application);
            _db.SaveChanges();

            StatusMessage = "Application has been deleted";
            return RedirectToPage("./Index");
        }

        IEnumerable<Breadcrumb.Breadcrumb> IBreadcrumbPage.Breadcrumbs()
        {
            return new[] { new Breadcrumb.Breadcrumb { Title = "Applications", Url = Url.Page("./Index") }, };
        }
    }
}