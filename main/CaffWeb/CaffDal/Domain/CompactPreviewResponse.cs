using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaffDal.Domain
{
    public class CompactPreviewResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Creator { get; set; }
        public DateTime CreationDate { get; set; }
        public byte[] Image { get; set; }
    }
}
