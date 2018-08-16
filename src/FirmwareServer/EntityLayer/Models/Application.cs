using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirmwareServer.EntityLayer.Models
{
    [Table("Application")]
    public class Application
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int DeviceTypeId { get; set; }

        public int? FirmwareId { get; set; }

        public DateTimeOffset Created { get; set; } = DateTimeOffset.Now;

        public string Name { get; set; }

        public string Description { get; set; }

        [ForeignKey("DeviceTypeId")]
        public virtual DeviceType DeviceType { get; set; }

        [ForeignKey("FirmwareId")]
        public virtual Firmware Firmware { get; set; }
    }
}
