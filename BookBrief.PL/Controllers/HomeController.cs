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
    [AllowAnonymous]
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


        public IActionResult Register()
        {
            return View();
        }

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

        public IActionResult Profile()
        {
            var userId = HttpContext.Session.GetInt32("_UserToken");
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

        public IActionResult BookDetail(int id)
        {

            var userId = HttpContext.Session.GetInt32("_UserToken");
            var _user = c.User.FirstOrDefault(x => x.Id == userId);
            ViewBag.user = _user;

            var result = c.Book.Where(x => x.BookId == id);
            return View(result);
        }
        public IActionResult ShareComment(int bookid, string text)
        {
            var userId = HttpContext.Session.GetInt32("_UserToken");

            Comment cmt = new Comment();
            cmt.UserId= userId.Value;
            cmt.Date = DateTime.Today;
            cmt.Text = text;
            cmt.BookId = bookid;
           // cmt._User= c.User.FirstOrDefault(x => x.Id == userId.Value);

            commentRepository.TAdd(cmt);

            return View("BookDetail",bookid);
        }


        public IActionResult Categories()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion
    }
}