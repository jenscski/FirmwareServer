using Microsoft.AspNetCore.Http;
using System;

namespace FirmwareServer.Extensions
{
    internal static class HttpRequestExtensions
    {
        internal static bool TryGetDeviceType(this HttpRequest request, out EntityLayer.ChipType chipType)
        {
            var userAgent = request.Headers["User-Agent"].ToString();

            if (string.Equals("ESP8266-http-Update", userAgent, StringComparison.OrdinalIgnoreCase))
            {
                chipType = EntityLayer.ChipType.ESP8266;
            }
            else
            {
                chipType = (EntityLayer.ChipType)int.MaxValue;
            }

            return chipType != (EntityLayer.ChipType)int.MaxValue;
        }
    }
}
