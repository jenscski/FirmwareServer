using FirmwareServer.EntityLayer;
using FirmwareServer.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FirmwareServer.Pages.Firmware
{
    public class UploadModel : PageModel, Breadcrumb.IBreadcrumbPage
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

            [Required]
            [Display(Name = "Firmware")]
            public IFormFile FirmwareFile { get; set; }

            [Display(Name = "Description")]
            public string Description { get; set; }
        }

        public UploadModel(Database db)
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
                var firmware = new EntityLayer.Models.Firmware
                {
                    Filename = Input.FirmwareFile.FileName,
                    Name = Input.Name,
                    DeviceTypeId = Input.DeviceTypeId.Value,
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

                StatusMessage = "New firmware has been uploaded";
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