using BookBrief.DL.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookBrief.PL.ViewComponents
{
    public class GetLastCategories : ViewComponent
    {

        Context c = new Context();
        public IViewComponentResult Invoke()
        {
            
            var resultt = c.Category.Take(5).ToList();
            return View(resultt);
        }
    }
}
