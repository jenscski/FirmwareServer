using FirmwareServer.Breadcrumb;
using FirmwareServer.EntityLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FirmwareServer.Pages.Devices
{
    public class EditModel : PageModel, Breadcrumb.IBreadcrumbPage
    {
        private readonly Database _db;

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public IEnumerable<object> DeviceTypesWithApplications => _db.DeviceTypes
            .Where(x => _db.Devices.Where(d => d.Id == Id && d.ChipType == x.ChipType).Any())
            .Where(x => x.Active || x.Id == Input.DeviceTypeId)
            .OrderBy(x => x.Name.ToLower())
            .Select(x => new
            {
                id = x.Id,
                name = x.Name,
                applications = _db.Applications
                    .Where(a => a.DeviceTypeId == x.Id)
                    .Select(a => new
                    {
                        id = a.Id,
                        name = a.Name
                    }),
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

            [Display(Name = "Application")]
            public int? ApplicationId { get; set; }

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
                ChipType = device.ChipType,
                DeviceTypeId = device.DeviceTypeId,
                ApplicationId = device.ApplicationId,
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
                device.Name = Input.Name;
                device.DeviceTypeId = Input.DeviceTypeId;
                device.ApplicationId = Input.ApplicationId;

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