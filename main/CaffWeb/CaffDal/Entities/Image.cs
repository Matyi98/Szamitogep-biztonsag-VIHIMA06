using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaffDal.Entities
{
    public class Image
    { 
        [Key]
        public int Id { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public int CaffId { get; set; }

        public byte[] Preview { get; set; }

    }
}
