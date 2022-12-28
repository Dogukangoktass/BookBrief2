using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookBrief.DL.Models
{
    public class BookCommentsVM
    {
        public IEnumerable<Comment> _Comment { get; set; }
        public IEnumerable<Book> _Book { get; set; }
    }
}
