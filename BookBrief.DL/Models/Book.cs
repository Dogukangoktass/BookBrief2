using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookBrief.DL.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }
        [Required(ErrorMessage = "Kitap Başlık Boş Geçilemez!")]
        [Display(Name = "Kitap Başlık ")]
        public string BookTitle { get; set; }

        [Required(ErrorMessage = "Açıklama Boş Geçilemez!")]
        [Display(Name = "Kitap Açıklama ")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Yazar Boş Geçilemez!")]
        [Display(Name = "Yazar")]
        public string Author { get; set; }
        [Required(ErrorMessage = "Kapak Resim Boş Geçilemez!")]
        [Display(Name = "Kitap Kapak Resim")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Özet Boş Geçilemez!")]
        [Display(Name = "Kitap Özet ")]
        public string Summary { get; set; }

        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Kitap Kategori Boş Geçilemez!")]
        [Display(Name = "Kitap Kategori ")]
        public int CategoryId { get; set; }
        public Category _Category { get; set; }

    }
}
