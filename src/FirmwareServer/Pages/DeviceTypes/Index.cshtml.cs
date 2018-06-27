using FirmwareServer.Breadcrumb;
using FirmwareServer.EntityLayer;
using FirmwareServer.EntityLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace FirmwareServer.Pages.DeviceTypes
{
    public class IndexModel : PageModel, IBreadcrumbPage
    {
        private readonly Database _db;

        [TempData]
        public string StatusMessage { get; set; }

        public IEnumerable<DeviceType> DeviceTypes { get; private set; }

        public IndexModel(Database db)
        {
            _db = db;
        }

        public void OnGet()
        {
            DeviceTypes = _db.DeviceTypes
                .OrderBy(x => x.ChipType)
                .ThenBy(x => x.Name)
                .ToList();
        }

        public IActionResult OnGetEnable(int id)
        {
            var row = _db.DeviceTypes.Find(id);
            if (row != null)
            {
                row.Active = true;
                _db.SaveChanges();
            }

            return RedirectToPage();
        }

        public IActionResult OnGetDisable(int id)
        {
            var row = _db.DeviceTypes.Find(id);
            if (row != null)
            {
                row.Active = false;
                _db.SaveChanges();
            }

            return RedirectToPage();
        }

        IEnumerable<Breadcrumb.Breadcrumb> IBreadcrumbPage.Breadcrumbs()
        {
            return null;
        }
    }
}