using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookBrief.DL.Models
{
    public class CommentsVM
    {
        public IEnumerable<Comment>_Comments { get; set; }
        public IEnumerable<UserModel> _User { get; set; }
        public IEnumerable<Book> _Book { get; set; }
    }
}
