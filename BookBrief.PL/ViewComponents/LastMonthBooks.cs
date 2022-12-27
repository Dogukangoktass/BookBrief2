using BookBrief.DL.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookBrief.PL.ViewComponents
{
    public class LastMonthBooks : ViewComponent
    {

        Context c = new Context();
        public IViewComponentResult Invoke()
        {
            var gecenay = DateTime.Today.AddDays(-30);
            var result = c.Book.Where(x => x.Date >= gecenay).ToList();
            TempData["SonKitapPaylasim"] = result;
            return View(result);
        }
    }
}

