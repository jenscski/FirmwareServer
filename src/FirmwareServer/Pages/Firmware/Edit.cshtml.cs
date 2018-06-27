using FirmwareServer.EntityLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FirmwareServer.Pages.Firmware
{
    public class EditModel : PageModel, Breadcrumb.IBreadcrumbPage
    {
        private readonly Database _db;

        [TempData]
        public string StatusMessage { get; set; }

        public IEnumerable<SelectListItem> DeviceTypes => _db.DeviceTypes
            .Where(x => x.Active || x.Id == Input.DeviceTypeId)
            .OrderBy(x => x.ChipType)
            .AsEnumerable()
            .GroupBy(x => x.ChipType)
            .Select(x => new
            {
                Group = new SelectListGroup { Name = x.Key.ToString() },
                Items = x.OrderBy(y => y.Name),
            })
            .SelectMany(x => x.Items, (g, i) => new SelectListItem
            {
                Group = g.Group,
                Value = i.Id.ToString(),
                Text = i.Name
            });

        [BindProperty]
        public InputModel Input { get; set; }

        public EditModel(Database db)
        {
            _db = db;
        }

        public class InputModel
        {
            [Required]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Required]
            [Display(Name = "Device type")]
            public int? DeviceTypeId { get; set; }

            [Display(Name = "Filename")]
            public string Filename { get; set; }

            [Display(Name = "Description")]
            public string Description { get; set; }
        }

        public void OnGet(int id)
        {
            var row = _db.Firmware.Find(id);
            if (row == null)
            {
                throw new ApplicationException($"Unable to load firmware with ID '{id}'.");
            }

            Input = new InputModel
            {
                Name = row.Name,
                Filename = row.Filename,
                Description = row.Description,
                DeviceTypeId = row.DeviceTypeId,
            };
        }

        public IActionResult OnPost(int id)
        {
            var row = _db.Firmware.Find(id);
            if (row == null)
            {
                throw new ApplicationException($"Unable to load firmware with ID '{id}'.");
            }

            if (ModelState.IsValid)
            {
                row.Name = Input.Name;
                row.DeviceTypeId = Input.DeviceTypeId.Value;
                row.Description = Input.Description;

                _db.SaveChanges();

                StatusMessage = "Firmware has been updated";
                return RedirectToPage("./Index");
            }

            return Page();
        }

        public IEnumerable<Breadcrumb.Breadcrumb> Breadcrumbs()
        {
            return new[] { new Breadcrumb.Breadcrumb { Title = "Firmware", Url = Url.Page("./Index") } };
        }
    }
}