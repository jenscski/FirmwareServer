using FirmwareServer.EntityLayer;
using FirmwareServer.EntityLayer.Models;
using FirmwareServer.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace FirmwareServer.Controllers
{
    [Route("api/[controller]")]
    public class FirmwareController : ControllerBase
    {
        private readonly Database _db;
        private readonly ILogger<FirmwareController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FirmwareController(Database db,
            ILogger<FirmwareController> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult GetFirmware()
        {
            var userAgent = Request.Headers["User-Agent"].ToString();

            _logger.LogWarning("Firmware request from device on IP {0}, with user agent {1}", _httpContextAccessor.HttpContext.Connection.RemoteIpAddress, userAgent);

            if (Request.TryGetDeviceType(out var deviceType))
            {
                switch (deviceType)
                {
                    case ChipType.ESP8266:
                        return HandleESP8266FirmwareRequest();
                }
            }

            _logger.LogWarning("Unknown device ({0}) trying to fetch firmware", userAgent);
            return BadRequest("Unknown device type");
        }

        private IActionResult HandleESP8266FirmwareRequest()
        {
            var stamac = Request.Headers["x-ESP8266-STA-MAC"].ToString().ToUpper();
            var apmac = Request.Headers["x-ESP8266-AP-MAC"].ToString().ToUpper();
            var freespace = Request.Headers["x-ESP8266-free-space"].ToString();
            var sketchsize = Request.Headers["x-ESP8266-sketch-size"].ToString();
            var sketchmd5 = Request.Headers["x-ESP8266-sketch-md5"].ToString().ToUpper();
            var chipsize = Request.Headers["x-ESP8266-chip-size"].ToString();
            var sdkversion = Request.Headers["x-ESP8266-sdk-version"].ToString();
            var version = Request.Headers["x-ESP8266-version"].ToString();

            var device = _db.Devices
                .Where(x => x.ChipType == ChipType.ESP8266)
                .Where(x => x.StaMac == stamac)
                .FirstOrDefault();

            if (device == null)
            {
                device = new Device
                {
                    ChipType = ChipType.ESP8266,
                    Name = stamac,
                    StaMac = stamac,
                    ApMac = apmac,
                };
                _db.Devices.Add(device);
                _db.SaveChanges();

                _logger.LogWarning("Created new device. ID={0}, MAC address={1}", device.Id, device.StaMac);
            }

            device.RemoteIpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            device.CurrentFirmwareId = _db.Firmware.Where(x => x.MD5 == sketchmd5).FirstOrDefault()?.Id;
            device.LastOnline = DateTimeOffset.Now;
            device.SdkVersion = sdkversion;
            device.Version = version;
            device.FreeSpace = int.TryParse(freespace, out var freespaceInt) ? freespaceInt : default(int?);
            device.ChipSize = int.TryParse(chipsize, out var chipsizeInt) ? chipsizeInt : default(int?);

            _db.SaveChanges();

            var firmware = _db.Firmware.Find(device.FirmwareId);
            if (firmware != null && !string.Equals(firmware.MD5, sketchmd5, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning("New firmware. ID={0}, Device ID={1}", firmware.Id, device.Id);
                return File(firmware.Data, "application/octet-stream");
            }

            return StatusCode(304);
        }
    }
}
