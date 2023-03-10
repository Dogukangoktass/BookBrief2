using BookBrief.BL.Repository;
using BookBrief.DL.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookBrief.PL.Controllers
{

    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        CategoryRepository categoryRepository = new CategoryRepository();
        BookRepository bookRepository = new BookRepository();
        CommentRepository commentRepository = new CommentRepository();
        UserRepository userRepository = new UserRepository();
        Context context = new Context();
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AdminController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public string GetCookie(string key)
        {
            HttpContext.Request.Cookies.TryGetValue(key, out string id);
            return id;
        }

        public IActionResult Index()
        {
            //var userId = HttpContext.Session.GetInt32("_UserToken");
            //var result = context.User.FirstOrDefault(x => x.Id == userId);
            //ViewBag.user = result;

            //ViewBag.kisi = GetCookie("userId");
            int id = Convert.ToInt16(GetCookie("userId"));
            var result = context.User.FirstOrDefault(x => x.Id == id);
            ViewBag.kisi = result;

            //var name = User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault().Value;


            TempData["KitapSayisi"] = context.Book.Count();
            TempData["KullaniciSayisi"] = context.User.Count();
            TempData["YorumSayisi"] = context.Comment.Count();
            TempData["KategoriSayisi"] = context.Category.Count();
            return View();
        }
        [HttpGet]
        public IActionResult Profile()
        {
            int id = Convert.ToInt16(GetCookie("userId"));
            var result = context.User.FirstOrDefault(x => x.Id == id);
            ViewBag.kisi = result;

            return View(result);
        }

        [HttpPost]
        public IActionResult ProfilEdit(UserModel us)
        {
            var x = userRepository.TGet(us.Id);
           
            
            if (ModelState.IsValid)
            {
                x.UserName = us.UserName;
                x.Email = us.Email;
                x.Password = us.Password;

                userRepository.TUpdate(x);
                return RedirectToAction("Profile");
            }
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

            x._Category = book._Category;
            //x._Category =;  // null geldiğinden modelstate isvalid false geliyor!!!!!!
            if (ModelState.IsValid)
            {
                x.BookTitle = book.BookTitle;
                x.Description = book.Description;
                x.Author = book.Author;

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
        public IActionResult BookDelete(int id) // view yok
        {
            var x = bookRepository.TGet(id);
            if (x == null)
            {
                return NotFound();
            }
            bookRepository.TDelete(x);
            return RedirectToAction("Books");
        }
        public IActionResult Comments()
        {
            //IEnumerable<Comment> comments = commentRepository.TList();

            var result = context.Comment.Where(x => x.IsShared == false).ToList();

            return View(result);

        }

        
        public IActionResult CommentsShared(int id)
        {
            var x = commentRepository.TGet(id);
            x.IsShared = true;
            commentRepository.TUpdate(x);
            return View("Index");
        }


        #region Category

        public IActionResult Category()
        {
            IEnumerable<Category> categories = categoryRepository.TList();
            return View(categories);
        }
        public IActionResult CategoryCreate()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CategoryCreate(Category ca)
        {
            if (ModelState.IsValid)
            {
                categoryRepository.TAdd(ca);
                return RedirectToAction("Category");
            }
            return View();
        }

        public IActionResult CategoryDelete(int id) // view yok
        {
            var x = categoryRepository.TGet(id);
            if (x == null)
            {
                return NotFound();
            }
            categoryRepository.TDelete(x);
            return RedirectToAction("Category");
        }
        public IActionResult CategoryEdit(int id)
        {

            var x = categoryRepository.TGet(id);
            if (id == 0)
            {
                return NotFound();
            }
            return View(x);
        }

        [HttpPost]
        public IActionResult CategoryEdit(Category cat)
        {
            var x = categoryRepository.TGet(cat.Id);
            if (ModelState.IsValid)
            {
                x.CategoryName = cat.CategoryName;
                x.Id = cat.Id;
                categoryRepository.TUpdate(x);
                return RedirectToAction("Category");
            }
            return View();
        }
        #endregion





        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            Response.Cookies.Delete("userId");
            return RedirectToAction("Index", "Home");
        }



    }
}
