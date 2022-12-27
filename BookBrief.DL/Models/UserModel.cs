using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookBrief.DL.Models
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Profil Resmi")]
        public string ImageUrl { get; set; }
        [Display(Name = "Kullanıcı Adı")]
        [Required(ErrorMessage = "Kullanıcı Adı Boş Geçilemez!")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Mail Adresi Boş Geçilemez!")]
        [Display(Name = "Mail Adresi")]
        public string Email { get; set; }
        [Display(Name = "Şifre")]

        [Required(ErrorMessage = "Şifre Boş Geçilemez!")]

        public int RoleId { get; set; }
        public Roles _Roles { get; set; }
        public string Password { get; set; }

    }
}
