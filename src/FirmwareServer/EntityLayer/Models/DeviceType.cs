using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FirmwareServer.EntityLayer.Models
{
    [Table("DeviceType")]
    public class DeviceType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public ChipType ChipType { get; set; }

        public string Name { get; set; }

        public bool Active { get; set; }
    }
}
