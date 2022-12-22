using BookBrief.BL.Repository;
using BookBrief.DL.Models;
using BookBrief.PL.Models;
using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace BookBrief.PL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        FirebaseAuthProvider auth;
        UserRepository user = new UserRepository();
        Context c = new Context();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            auth = new FirebaseAuthProvider(
                        new FirebaseConfig("AIzaSyDWyQN8lInBQ8GlmZeXRMGURK4nJLwQoWc"));
        }

        public IActionResult Index()
        {
            // denemes
            // deneme2
            return View();
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
                TempData["MailOnay"] = "Lütfen Mail Adresinizi Onaylayınız";

                _user.UserName = loginModel.Name;
                _user.Password = loginModel.Password;
                _user.Email = loginModel.Email;
                _user.ImageUrl = "url";
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
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string Email, string Password)
        {
            try
            {
                //log in an existing user
                var fbAuthLink = await auth
                                .SignInWithEmailAndPasswordAsync(Email, Password);
                TempData["userName"] = Email;
                string token = fbAuthLink.FirebaseToken;
                //save the token to a session variable
                if (token != null)
                {
                    // var id= user.GetUser(loginModel.Email);

                    var result = c.User.Where(x => x.Email == Email).FirstOrDefault();

                    TempData["result"] = result.UserName;


                    // HttpContext.Session.SetInt32("_UserToken", result.Id);
                    return RedirectToAction("Index", "Admin");
                }

            }
            catch (FirebaseAuthException ex)
            {
                var firebaseEx = JsonConvert.DeserializeObject<FirebaseError>(ex.ResponseData);

                TempData["Error"] = "Mail adresi veya şifre hatalı!";
                return View();
            }

            return View();
        }










        #region PAGES

      
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