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
            var result = c.Book.Distinct().OrderByDescending(d => d.Date);
            return View(result);
        }
    }
}
