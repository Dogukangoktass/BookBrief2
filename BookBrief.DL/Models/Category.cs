using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookBrief.DL.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Kitap Kategori")]
        public string CategoryName { get; set; }
    }
}
