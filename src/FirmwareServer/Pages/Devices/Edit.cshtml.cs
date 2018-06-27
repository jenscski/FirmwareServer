using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FirmwareServer.Breadcrumb;
using FirmwareServer.EntityLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FirmwareServer.Pages.Devices
{
    public class EditModel : PageModel, Breadcrumb.IBreadcrumbPage
    {
        private readonly Database _db;

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public IEnumerable<SelectListItem> DeviceTypes => _db.DeviceTypes
            .Where(x => _db.Devices.Where(d => d.Id == Id && d.ChipType == x.ChipType).Any())
            .Where(x => x.Active || x.Id == Input.DeviceTypeId)
            .OrderBy(x => x.Name.ToLower())
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name,
            })
            .ToList();

        public IEnumerable<SelectListItem> Firmware => _db.Firmware
            .Where(x => _db.Devices.Where(d => d.Id == Id && d.DeviceTypeId == x.DeviceTypeId).Any())
            .OrderBy(x => x.Name.ToLower())
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name,
            })
            .ToList();

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Display(Name = "Device type")]
            public int? DeviceTypeId { get; set; }

            [Display(Name = "Firmware")]
            public int? FirmwareId { get; set; }

            [Display(Name = "Chip type")]
            public ChipType ChipType { get; internal set; }
        }

        public EditModel(Database db)
        {
            _db = db;
        }

        public void OnGet()
        {
            var device = _db.Devices.Find(Id);
            if (device == null)
            {
                throw new ApplicationException($"Unable to load device with ID '{Id}'.");
            }

            Input = new InputModel
            {
                Name = device.Name,
                DeviceTypeId = device.DeviceTypeId,
                FirmwareId = device.FirmwareId,
                ChipType = device.ChipType,
            };
        }

        public IActionResult OnPost()
        {
            var device = _db.Devices.Find(Id);
            if (device == null)
            {
                throw new ApplicationException($"Unable to load device with ID '{Id}'.");
            }

            if (ModelState.IsValid)
            {
                device.DeviceTypeId = Input.DeviceTypeId;
                device.Name = Input.Name;
                device.FirmwareId = Input.FirmwareId;

                _db.SaveChanges();

                StatusMessage = "Device has been updated";
                return RedirectToPage("./Index");
            }

            return Page();
        }

        IEnumerable<Breadcrumb.Breadcrumb> IBreadcrumbPage.Breadcrumbs()
        {
            return new[] {
                new Breadcrumb.Breadcrumb { Title = "Devices", Url = Url.Page("./Index") },
            };
        }
    }
}