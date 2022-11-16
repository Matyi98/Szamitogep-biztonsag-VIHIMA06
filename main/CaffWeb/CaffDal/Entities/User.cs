using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace CaffDal.Entities
{
    public partial class User : IdentityUser<int>
    {
        [Required]
        [MaxLength(40), MinLength(3)]
        [Column(TypeName = "nvarchar(40)")]
        public string CustomName { get; set; }
    }
}
