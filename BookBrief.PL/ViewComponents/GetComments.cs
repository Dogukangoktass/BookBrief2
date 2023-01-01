using BookBrief.BL.Repository;
using BookBrief.DL.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookBrief.PL.ViewComponents
{
    public class GetComments : ViewComponent
    {
        Context c = new Context();
        CommentRepository commentRepository = new CommentRepository();
        UserRepository userRepository = new UserRepository();
        public IViewComponentResult Invoke(int id)
        {
            var result = c.Comment.Where(x => x.BookId == id).ToList();
            return View(result);
        }
    }
}
