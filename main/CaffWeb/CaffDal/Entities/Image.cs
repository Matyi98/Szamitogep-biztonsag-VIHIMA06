using System.ComponentModel.DataAnnotations;

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
