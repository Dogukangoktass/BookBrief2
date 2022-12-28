using BookBrief.BL.Repository;
using BookBrief.DL.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookBrief.PL.ViewComponents
{
    public class BookGet : ViewComponent
    {
        BookRepository bookRepository = new BookRepository();
        Context c = new Context();
        public IViewComponentResult Invoke()
        {
            var result = c.Book.Distinct().OrderByDescending(d => d.Date).Take(6);
            //Sorguda en çok yorum alandan en aza doğru 6 adet listeleyecek!!!!




            return View(result);
        }
    }
}
