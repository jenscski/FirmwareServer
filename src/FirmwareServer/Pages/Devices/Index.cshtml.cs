﻿using FirmwareServer.Breadcrumb;
using FirmwareServer.EntityLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FirmwareServer.Pages.Devices
{
    public class IndexModel : PageModel, IBreadcrumbPage
    {
        private readonly Database _db;

        [TempData]
        public string StatusMessage { get; set; }

        public IEnumerable<RowModel> Devices { get; private set; }

        public class RowModel
        {
            public int Id { get; internal set; }

            public string Name { get; internal set; }

            public string DeviceType { get; internal set; }

            public int? CurrentFirmwareId { get; internal set; }

            public DateTimeOffset LastOnline { get; internal set; }

            public ChipType ChipType { get; internal set; }

            public int? FirmwareId { get; internal set; }
            public string Application { get; internal set; }
        }

        public IndexModel(Database db)
        {
            _db = db;
        }

        public void OnGet()
        {
            Devices = _db.Devices
                .Select(x => new RowModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    ChipType = x.ChipType,
                    LastOnline = x.LastOnline,
                    FirmwareId = x.Application.FirmwareId,
                    CurrentFirmwareId = x.CurrentFirmwareId,

                    DeviceType = x.DeviceType.Name,
                    Application = x.Application.Name,
                })
                .OrderBy(x => x.Name.ToLower())
                .ToList();
        }

        IEnumerable<Breadcrumb.Breadcrumb> IBreadcrumbPage.Breadcrumbs()
        {
            return null;
        }
    }
}