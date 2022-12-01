using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaffDal.Entities
{
    public partial class User : IdentityUser<int>
    {
        [Required]
        [MaxLength(40), MinLength(3)]
        [Column(TypeName = "nvarchar(40)")]
        public string CustomName { get; set; }
        public ICollection<Caff> Caffs { get; set; } = new List<Caff>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
