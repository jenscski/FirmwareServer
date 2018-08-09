using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirmwareServer.EntityLayer.Models
{
    [Table("Device")]
    public class Device
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? DeviceTypeId { get; set; }

        public DateTimeOffset Created { get; set; } = DateTimeOffset.Now;

        public DateTimeOffset LastOnline { get; set; }

        public string Name { get; set; }

        public string StaMac { get; set; }

        public string ApMac { get; set; }

        public string SdkVersion { get; set; }

        public string Version { get; set; }

        public int? FirmwareId { get; set; }

        public int? CurrentFirmwareId { get; set; }

        public int? FreeSpace { get; set; }

        public int? ChipSize { get; set; }

        public string RemoteIpAddress { get; set; }

        public ChipType ChipType { get; set; }

        [ForeignKey("DeviceTypeId")]
        public virtual DeviceType DeviceType { get; set; }

        [ForeignKey("FirmwareId")]
        public virtual Firmware Firmware { get; set; }

        [ForeignKey("CurrentFirmwareId")]
        public virtual Firmware CurrentFirmware { get; set; }
    }
}