using BookBrief.BL.Repository;
using BookBrief.DL.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookBrief.PL.ViewComponents
{
    public class MostCommented:ViewComponent
    {
        BookRepository bookRepository = new BookRepository();
        Context c = new Context();
        public IViewComponentResult Invoke()
        {
            var result = c.Book.Distinct().OrderByDescending(d => d.Date).Take(6);


            /*
             var results = db.siparis_urunler.GroupBy(g => g.urun_id).Where(w => w.Count()>1)
.Select(s =>
new {
s.FirstOrDefault().urunler.urun_adi,
adet = s.Count()
}).Distinct().Take(5).OrderByDesceding(o=>o.adet).ToList();
             */

             



            return View(result);
        }
    }
}
