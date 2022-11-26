using System.ComponentModel.DataAnnotations;

namespace CaffDal.Entities
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }
        public int UserId { get; set; }
        public int CaffId { get; set; }
        public Caff Caff { get; set; } = null!;
        public User User { get; set; } = null!;

        public Comment(string text)
        {
            Text = text;
        }
    }
}
