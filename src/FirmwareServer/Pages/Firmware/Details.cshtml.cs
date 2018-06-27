using FirmwareServer.EntityLayer;
using FirmwareServer.EntityLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FirmwareServer.Pages.Firmware
{
    public class DetailsModel : PageModel, Breadcrumb.IBreadcrumbPage
    {
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public EntityLayer.Models.Firmware Firmware { get; private set; }

        public List<Device> Devices { get; private set; }

        public void OnGet([FromServices] Database db)
        {
            Firmware = db.Firmware.Find(Id);
            if (Firmware == null)
            {
                throw new ApplicationException($"Unable to load firmware with ID '{Id}'.");
            }

            Devices = db.Devices.Where(x => x.CurrentFirmwareId == Id).OrderBy(x => x.Name.ToLower()).ToList();
        }

        public IEnumerable<Breadcrumb.Breadcrumb> Breadcrumbs()
        {
            return new[] { new Breadcrumb.Breadcrumb { Title = "Firmware", Url = Url.Page("./Index") } };
        }
    }
}