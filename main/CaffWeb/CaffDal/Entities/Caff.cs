using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaffDal.Entities
{
    public class Caff
    {
        [Key]
        public int Id { get; set; }

        public string Creator { get; set; }

        public DateTime CreatorDate { get; set; }

        public int NumberOfFrames { get; set; }

        public byte[] RawCaff { get; set; }

        public virtual ICollection<Image> Images { get; set; }
    }
}
