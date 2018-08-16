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

namespace FirmwareServer.Pages.Applications
{
    public class DetailsModel : PageModel, IBreadcrumbPage
    {
        private readonly Database _db;

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public Application Application { get; private set; }

        public IEnumerable<EntityLayer.Models.Firmware> Firmware => _db.Firmware
            .Where(x => x.ApplicationId == Id)
            .OrderByDescending(x => x.Created);

        public DetailsModel(Database db)
        {
            _db = db;
        }

        public void OnGet()
        {
            Application = _db.Applications
                .Include(x => x.DeviceType)
                .Include(x => x.Firmware)
                .First(x => x.Id == Id);
            if (Application == null)
            {
                throw new ApplicationException($"Unable to load application with ID '{Id}'.");
            }
        }

        public IEnumerable<Breadcrumb.Breadcrumb> Breadcrumbs()
        {
            return new[] { new Breadcrumb.Breadcrumb { Title = "Applications", Url = Url.Page("./Index") } };
        }
    }
}