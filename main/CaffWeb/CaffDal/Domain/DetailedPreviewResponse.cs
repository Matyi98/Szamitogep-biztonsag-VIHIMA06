using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaffDal.Domain
{
    public class DetailedPreviewResponse
    {
        public string Name { get; set; }
        public string Creator { get; set; }
        public DateTime CreatorDate { get; set; }
        public byte[] Image { get; set; }

        public int CaffID { get; set; }
        public int CreatorID { get; set; }
    }
}
