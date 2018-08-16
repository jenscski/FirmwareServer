using FirmwareServer.EntityLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;

namespace FirmwareServer.Pages.Firmware
{
    public class DetailsModel : PageModel, Breadcrumb.IBreadcrumbPage
    {
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public EntityLayer.Models.Firmware Firmware { get; private set; }
        public EntityLayer.Models.Application Application { get; private set; }

        public void OnGet([FromServices] Database db)
        {
            Firmware = db.Firmware.Find(Id);
            if (Firmware == null)
            {
                throw new ApplicationException($"Unable to load firmware with ID '{Id}'.");
            }

            Application = db.Applications.Find(Firmware.ApplicationId);
            if (Application == null)
            {
                throw new ApplicationException($"Unable to load application with ID '{Firmware.ApplicationId}'.");
            }
        }

        public IActionResult OnGetDownload([FromServices] Database db)
        {
            Firmware = db.Firmware.Find(Id);
            if (Firmware == null)
            {
                throw new ApplicationException($"Unable to load firmware with ID '{Id}'.");
            }

            return File(Firmware.Data, "application/octet-stream", Firmware.Filename);
        }



        public IEnumerable<Breadcrumb.Breadcrumb> Breadcrumbs()
        {
            return new[] {
                new Breadcrumb.Breadcrumb { Title = "Applications", Url = Url.Page("/Applications/Index") },
                new Breadcrumb.Breadcrumb { Title = Application.Name, Url = Url.Page("/Applications/Details", new{ id=Application.Id }) }
            };
        }
    }
}