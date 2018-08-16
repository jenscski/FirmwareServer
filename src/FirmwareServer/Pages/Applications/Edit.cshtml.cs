using FirmwareServer.Breadcrumb;
using FirmwareServer.EntityLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FirmwareServer.Pages.Applications
{
    public class EditModel : PageModel, IBreadcrumbPage
    {
        private readonly Database _db;

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

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

        public IEnumerable<SelectListItem> Firmware => _db.Firmware
            .Where(x => x.ApplicationId == Id)
            .OrderByDescending(x => x.Created)
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name,
            });

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Required]
            [Display(Name = "Device type")]
            public int? DeviceTypeId { get; set; }

            [Display(Name = "Firmware")]
            public int? FirmwareId { get; set; }

            [Display(Name = "Description")]
            public string Description { get; set; }
        }

        public EditModel(Database db)
        {
            _db = db;
        }

        public void OnGet()
        {
            var application = _db.Applications.Find(Id);
            if (application == null)
            {
                throw new ApplicationException($"Unable to load application with ID '{Id}'.");
            }

            Input = new InputModel
            {
                Name = application.Name,
                DeviceTypeId = application.DeviceTypeId,
                Description = application.Description,
                FirmwareId = application.FirmwareId,
            };
        }

        public IActionResult OnPost()
        {
            var application = _db.Applications.Find(Id);
            if (application == null)
            {
                throw new ApplicationException($"Unable to load application with ID '{Id}'.");
            }

            if (ModelState.IsValid)
            {
                application.Name = Input.Name;
                application.Description = Input.Description;
                application.DeviceTypeId = Input.DeviceTypeId.Value;
                application.FirmwareId = Input.FirmwareId;

                _db.SaveChanges();

                StatusMessage = "Application has been updated";
                return RedirectToPage("./Index");
            }

            return Page();
        }

        IEnumerable<Breadcrumb.Breadcrumb> IBreadcrumbPage.Breadcrumbs()
        {
            return new[] {
                new Breadcrumb.Breadcrumb { Title = "Applications", Url = Url.Page("./Index") },
            };
        }
    }
}