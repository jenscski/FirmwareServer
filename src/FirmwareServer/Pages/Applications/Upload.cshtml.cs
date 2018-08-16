using FirmwareServer.Breadcrumb;
using FirmwareServer.EntityLayer;
using FirmwareServer.EntityLayer.Models;
using FirmwareServer.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FirmwareServer.Pages.Applications
{
    public class UploadModel : PageModel, IBreadcrumbPage
    {
        private Database _db;

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public Application Application { get; private set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Required]
            [Display(Name = "Firmware")]
            public IFormFile FirmwareFile { get; set; }

            [Display(Name = "Description")]
            public string Description { get; set; }

            [Display(Name = "Make active on application")]
            public bool MakeActive { get; set; }
        }

        public UploadModel(Database db)
        {
            _db = db;
        }

        public void OnGet()
        {
            Application = _db.Applications.Find(Id);
            if (Application == null)
            {
                throw new ApplicationException($"Unable to load application with ID '{Id}'.");
            }
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                Application = _db.Applications.Find(Id);
                if (Application == null)
                {
                    throw new ApplicationException($"Unable to load application with ID '{Id}'.");
                }

                var firmware = new EntityLayer.Models.Firmware
                {
                    Filename = Input.FirmwareFile.FileName,
                    Name = Input.Name,
                    ApplicationId = Id,
                    Description = Input.Description,
                };

                using (var mem = new System.IO.MemoryStream())
                {
                    Input.FirmwareFile.CopyTo(mem);
                    firmware.Data = mem.ToArray();
                }

                firmware.MD5 = firmware.Data.ComputeMD5Hash();

                //TODO prevent duplicate firmware upload

                _db.Firmware.Add(firmware);
                _db.SaveChanges();

                if (Input.MakeActive)
                {
                    Application.FirmwareId = firmware.Id;
                    _db.SaveChanges();
                }

                StatusMessage = "New firmware has been uploaded";
                return RedirectToPage("./Details");
            }

            return Page();
        }

        public IEnumerable<Breadcrumb.Breadcrumb> Breadcrumbs()
        {
            return new[] {
                new Breadcrumb.Breadcrumb { Title = "Applications", Url = Url.Page("./Index") },
                new Breadcrumb.Breadcrumb { Title = Application.Name, Url = Url.Page("./Details", new{ id=Application.Id }) }
            };
        }
    }
}