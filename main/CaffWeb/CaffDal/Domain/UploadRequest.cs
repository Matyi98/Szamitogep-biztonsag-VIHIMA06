using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaffDal.Domain
{
    public class UploadRequest
    {
        public int OwnerId { get; set; }
        public string CaffName { get; set; }
        public byte[] RawCaff { get; set; }

    }
}
