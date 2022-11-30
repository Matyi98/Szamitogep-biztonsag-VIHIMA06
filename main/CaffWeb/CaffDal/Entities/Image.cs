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

        public string? Caption { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        //public List<string> Tags { get; set; } = new List<string>();

        [Required]
        public int CaffId { get; set; }
        public Caff Caff { get; set; } = null!;

        public byte[] Preview { get; set; }

        public Image(byte[] preview)
        {
            Preview = preview;
        }

    }
}

/*
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
*/