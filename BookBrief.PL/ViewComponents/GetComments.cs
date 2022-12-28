using BookBrief.BL.Repository;
using BookBrief.DL.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookBrief.PL.ViewComponents
{
    public class GetComments : ViewComponent
    {

        Context c = new Context();
        CommentRepository commentRepository = new CommentRepository();
        public IViewComponentResult Invoke()
        {
            //var resultt =
            return View();
        }
    }
}
