using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaffDal.Domain
{
    public class CommentResponse
    {
        public int Id { get; set; }
        public int CommenterId { get; set; }
        public string Commenter { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
