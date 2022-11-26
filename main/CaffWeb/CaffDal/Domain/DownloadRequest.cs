using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaffDal.Domain
{
    public class DownloadRequest
    {
        public byte[] Bytes{ get; set; }
        public string Name{ get; set; }
    }
}
