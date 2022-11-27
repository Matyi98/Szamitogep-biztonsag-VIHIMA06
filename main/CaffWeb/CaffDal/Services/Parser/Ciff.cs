using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaffDal.Services.Parser
{
    public class Ciff
    {
        public int Duration { get; set;}
        public string? Caption { get; set;}
        public List<string> Tags { get; set; } = new List<string>();
        public int Width { get; set; }
        public int Height { get; set; }
        public byte[] RawCiff { get; set; }

        public Ciff(byte[] rawCiff)
        {
            RawCiff = rawCiff;
        }
    }
}
