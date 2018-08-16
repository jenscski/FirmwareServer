using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirmwareServer.EntityLayer.Models
{
    [Table("Firmware")]
    public class Firmware
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTimeOffset Created { get; set; } = DateTimeOffset.Now;

        public int ApplicationId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Filename { get; set; }

        public byte[] Data { get; set; }

        public string MD5 { get; set; }

        [ForeignKey("ApplicationId")]
        public virtual Application Application { get; set; }
    }
}