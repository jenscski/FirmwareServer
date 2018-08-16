using FirmwareServer.EntityLayer;
using FirmwareServer.EntityLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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

        [BindProperty]
        public InputModel Input { get; set; }
        public Application Application { get; private set; }

        public EditModel(Database db)
        {
            _db = db;
        }

        public class InputModel
        {
            [Required]
            [Display(Name = "Name")]
            public string Name { get; set; }

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

            Application = _db.Applications.Find(row.ApplicationId);
            if (Application == null)
            {
                throw new ApplicationException($"Unable to load application with ID '{row.ApplicationId}'.");
            }

            Input = new InputModel
            {
                Name = row.Name,
                Filename = row.Filename,
                Description = row.Description,
            };
        }

        public IActionResult OnPost(int id)
        {
            var row = _db.Firmware.Find(id);
            if (row == null)
            {
                throw new ApplicationException($"Unable to load firmware with ID '{id}'.");
            }

            Application = _db.Applications.Find(row.ApplicationId);
            if (Application == null)
            {
                throw new ApplicationException($"Unable to load application with ID '{row.ApplicationId}'.");
            }

            if (ModelState.IsValid)
            {
                row.Name = Input.Name;
                row.Description = Input.Description;

                _db.SaveChanges();

                StatusMessage = "Firmware has been updated";
                return RedirectToPage("/Applications/Details", new { id = row.ApplicationId });
            }

            return Page();
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