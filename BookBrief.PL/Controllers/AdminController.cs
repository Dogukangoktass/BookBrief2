using BookBrief.BL.Repository;
using BookBrief.DL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookBrief.PL.Controllers
{
    public class AdminController : Controller
    {
        CategoryRepository categoryRepository = new CategoryRepository();
        BookRepository bookRepository = new BookRepository();
        Context context = new Context();
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AdminController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Books()
        {
            IEnumerable<Book> _blog = context.Book.Include(x => x._Category);
            return View(_blog);
        }

        public IActionResult BookDetail(int id)
        {
            var result = bookRepository.TGet(id);
            return View(result);
        }
        public IActionResult BookEdit(int id)
        {
            var result = bookRepository.TGet(id);

            List<SelectListItem> values = (from x in context.Category.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.CategoryName,
                                               Value = x.Id.ToString(),
                                           }).ToList();
            ViewBag.v = values;

            if (id == 0)
            {
                return NotFound();
            }
            else
            {
                return View(result);
            }
        }

        [HttpPost]
        public IActionResult BookEdit(Book book)
        {

            var x = bookRepository.TGet(book.BookId);
            x.ImageUrl = book.ImageUrl;
            x.Date = book.Date;  // yayınlama tarihi (değişmese de olur)
            x._Category = book._Category;  // null geldiğinden modelstate isvalid false geliyor!!!!!!
            if (ModelState.IsValid)
            {
                x.BookTitle = book.BookTitle;
                x.Description = book.Description;
                x.Author=book.Author;
                
                x.Summary = book.Summary;
                x.CategoryId = book.CategoryId;
              
                bookRepository.TUpdate(x);
               
                return RedirectToAction("Books");
            }
            return View();
        }

        public IActionResult BookCreate()
        {
            List<SelectListItem> values = (from x in context.Category.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.CategoryName,
                                               Value = x.Id.ToString(),
                                           }).ToList();
            ViewBag.v = values;
            return View();
        }
        [HttpPost]
        public IActionResult BookCreate(BookAdd b)
        {
            Book bl = new Book();
            if (ModelState.IsValid)
            {
                if (b.ImageUrl != null)
                {
                    var extension = Path.GetExtension(b.ImageUrl.FileName);
                    var newImageName = Guid.NewGuid() + extension;
                    var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/", newImageName);
                    var stream = new FileStream(location, FileMode.Create);
                    b.ImageUrl.CopyTo(stream);
                    bl.ImageUrl = newImageName;
                }
                bl.BookTitle = b.BookTitle;
                bl.Description = b.Description;
                bl.Author = b.Author;
                bl.Summary = b.Summary;
                bl.CategoryId = b.CategoryId;
                bl.Date = b.Date;

                bookRepository.TAdd(bl);
                return RedirectToAction("Books");
            }
            return View();
        }
        public IActionResult Comments()
        {
            return View();
        }
        public IActionResult Category()
        {
            IEnumerable<Category> categories = categoryRepository.TList();
            return View(categories);
        }

    }
}
