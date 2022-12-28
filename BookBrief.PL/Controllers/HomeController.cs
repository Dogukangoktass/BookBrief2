using BookBrief.BL.Repository;
using BookBrief.DL.Models;
using BookBrief.PL.Models;
using Firebase.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
namespace BookBrief.PL.Controllers
{
   
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        FirebaseAuthProvider auth;
        UserRepository user = new UserRepository();
        CommentRepository commentRepository = new CommentRepository();
        Context c = new Context();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            auth = new FirebaseAuthProvider(
                        new FirebaseConfig("AIzaSyDWyQN8lInBQ8GlmZeXRMGURK4nJLwQoWc"));

        }
        [AllowAnonymous]
        public IActionResult Index()
        {

          
            ViewBag.kisi= GetCookie("userId");
            return View();
        }

        public void SetCookie(string key, string id)
        {
            HttpContext.Response.Cookies.Append(key, id);
        }

        public string GetCookie(string key)
        {
            HttpContext.Request.Cookies.TryGetValue(key, out string id);
            return id;
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [AllowAnonymous]

        [HttpPost]
        public async Task<IActionResult> Register(SignUp loginModel)
        {
            UserModel _user = new UserModel();
            try
            {
                var a = await auth.CreateUserWithEmailAndPasswordAsync(loginModel.Email, loginModel.Password, loginModel.Name, false);

                _user.UserName = loginModel.Name;
                _user.Password = loginModel.Password;
                _user.Email = loginModel.Email;
                _user.ImageUrl = "url";
                _user.RoleName = "User";
                if (ModelState.IsValid)
                {
                    user.TAdd(_user);
                    return RedirectToAction("Login");
                }
            }
            catch (Exception ex)
            {
                if (Response.StatusCode == 200)
                {
                    TempData["Hata"] = "E mail mevcut!";
                }
            }

            return View();

        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            //ClaimsPrincipal claimUser = HttpContext.User;

            //if (claimUser.Identity.IsAuthenticated)
            //{
            //    return RedirectToAction("Index", "Admin");
            //}
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string Email, string Password)
        {

            Context c = new Context();
            var dataValue = c.User.FirstOrDefault(x => x.Email == Email && x.Password == Password);


            if (dataValue != null)
            {
                if (dataValue.RoleName == "Admin")
                {
                    var claims = new List<Claim> {
                        new Claim(ClaimTypes.Name,Email),
                        new Claim(ClaimTypes.Role, "Admin")
                    };
                    var userIdentity = new ClaimsIdentity(claims, "a");
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(userIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);
                     HttpContext.Session.SetInt32("_UserToken", dataValue.Id);


                    // HttpCookie cookieVisitor = new HttpCookie(name, value);
                    SetCookie("userId", dataValue.Id.ToString());



                    return RedirectToAction("Index", "Admin");

                }
                else
                {
                    var claims = new List<Claim> {

                        new Claim(ClaimTypes.Name,Email),
                        new Claim(ClaimTypes.Role, "User")
                    };
                    var userIdentity = new ClaimsIdentity(claims, "a");
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(userIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);
                    HttpContext.Session.SetInt32("_UserToken", dataValue.Id);
                    SetCookie("userId", dataValue.Id.ToString());
                    return RedirectToAction("Index", "Home");
                }

            }
            else
            {
                TempData["ErrorMessage"] = "Kullanıcı email ya da şifre girmediniz lütfen kontrol ediniz.";

            }
            return View();


        }

        [Authorize(Roles ="User")]
        public IActionResult Profile()
        {
            int userId =  Convert.ToInt16(GetCookie("userId"));
            var result = c.User.FirstOrDefault(x => x.Id == userId);
            ViewBag.user = result;

            return View();
        }

        public async Task<IActionResult> LogOut()
        {
           await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Cookies.Delete("userId");

            return RedirectToAction("Index", "Home");
        }



        #region PAGES

        [AllowAnonymous]
        public IActionResult BookDetail(int id)
        {
            BookCommentsVM bcomments = new BookCommentsVM();

            bcomments._Book = c.Book.Where(x => x.BookId == id);
            bcomments._Comment = c.Comment.Where(x => x.BookId == id);

             var result = c.Book.Where(x => x.BookId == id);
            return View(result);
        }
        [AllowAnonymous]
        public IActionResult ShareComment(int bookid, string text)
        {
            int userId = Convert.ToInt16(GetCookie("userId"));
            var result = c.User.FirstOrDefault(x => x.Id == userId);

            Comment cmt = new Comment();
            cmt.UserId= userId;
            cmt.Date = DateTime.Today;
            cmt.Text = text;
            cmt.BookId = bookid;
            cmt.IsShared = false;
            // cmt._User= c.User.FirstOrDefault(x => x.Id == userId.Value);
            if (ModelState.IsValid)
            {
                commentRepository.TAdd(cmt);
                TempData["yorum"] = 1;
            }

            return RedirectToAction("BookDetail", new { id = bookid });
        }

        [AllowAnonymous]
        public IActionResult Categories()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult About()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Contact()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion
    }
}