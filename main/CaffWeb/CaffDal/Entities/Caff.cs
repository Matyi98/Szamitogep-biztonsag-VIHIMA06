using System.ComponentModel.DataAnnotations;

namespace CaffDal.Entities
{
    public class Caff
    {
        [Key]
        public int Id { get; set; }

        public string Creator { get; set; }

        public DateTime CreatorDate { get; set; }

        public int NumberOfFrames { get; set; }

        public string CaffName { get; set; }
        public byte[] RawCaff { get; set; }
        public ICollection<Image> Images { get; set; } = new List<Image>();
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public Caff(string creator, byte[] rawCaff)
        {
            Creator = creator;
            RawCaff = rawCaff;
        }

    }
}
