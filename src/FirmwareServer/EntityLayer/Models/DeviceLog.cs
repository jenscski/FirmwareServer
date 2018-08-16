using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirmwareServer.EntityLayer.Models
{
    [Table("DeviceLog")]
    public class DeviceLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTimeOffset Created { get; set; } = DateTimeOffset.Now;

        public int? DeviceId { get; set; }

        public LogLevel Level { get; set; } = LogLevel.Information;

        public string Message { get; set; }

        [ForeignKey("DeviceId")]
        public virtual Device Device { get; set; }
    }
}
