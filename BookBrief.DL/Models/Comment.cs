using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookBrief.DL.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Yorum")]
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public int BookId { get; set; }
        public Book _Book { get; set; }

        public int UserId { get; set; }
        //public UserModel _User { get; set; }

    }
}
