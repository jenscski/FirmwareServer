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

namespace FirmwareServer.Pages.Applications
{
    public class CreateModel : PageModel, IBreadcrumbPage
    {
        private Database _db;

        [TempData]
        public string StatusMessage { get; set; }

        public IEnumerable<SelectListItem> DeviceTypes => _db.DeviceTypes
            .Where(x => x.Active)
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

        public class InputModel
        {
            [Required]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Required]
            [Display(Name = "Device type")]
            public int? DeviceTypeId { get; set; }

            [Display(Name = "Description")]
            public string Description { get; set; }
        }

        public CreateModel(Database db)
        {
            _db = db;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var application = new EntityLayer.Models.Application
                {
                    Name = Input.Name,
                    DeviceTypeId = Input.DeviceTypeId.Value,
                    Description = Input.Description,
                };

                _db.Applications.Add(application);
                _db.SaveChanges();

                StatusMessage = "New application has been created";
                return RedirectToPage("./Details", new { id = application.Id });
            }

            return Page();
        }

        public IEnumerable<Breadcrumb.Breadcrumb> Breadcrumbs()
        {
            return new[] { new Breadcrumb.Breadcrumb { Title = "Applications", Url = Url.Page("./Index") } };
        }
    }
}