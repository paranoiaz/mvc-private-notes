using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Data;
using Helpers;
using Models;

namespace private_notes.Controllers
{
    public class HomeController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public HomeController(DatabaseContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this._context = context;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        [HttpGet]
        public ActionResult Index()
        {
            // redirect to /notes/index if authenticated 
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("index", "notes");
            }

            return View();
        }

        [HttpGet]
        public ActionResult Login(string error = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("index", "notes");
            }

            ViewData["Warning"] = error ?? "";

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(string username, string password)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("index", "notes");
            }

            var user = await this._userManager.FindByNameAsync(username);

            if (user == null)
            {
                return RedirectToAction("login", new { error = "This user does not exist." });
            }

            if (user.UserName.ToLower().Equals(username) && user.PasswordHash.Equals(HashingAlgorithm.GetHashString(password)))
            {
                await this._signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("index", "notes");
            }

            return RedirectToAction("login", new { error = "Login details are incorrect." });
        }

        [HttpGet]
        public ActionResult SignUp(string error = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("index", "notes");
            }

            ViewData["Warning"] = error ?? "";

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SignUp(string username, string password)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("index", "notes");
            }

            var user = await this._userManager.FindByNameAsync(username);

            if (user != null)
            {
                return RedirectToAction("signup", new { error = "Username already taken." });
            }

            if (username.Length > 16)
            {
                return RedirectToAction("signup", new { error = "Username maximum 16 characters." });
            }

            if (password.Length < 8)
            {
                return RedirectToAction("signup", new { error = "Password minimum 8 characters." });
            }

            var identityUser = new User { UserName = username.ToLower(), PasswordHash = HashingAlgorithm.GetHashString(password) };
            var result = await this._userManager.CreateAsync(identityUser);

            if (result.Succeeded)
            {
                return RedirectToAction("login");
            }
            else
            {
                return RedirectToAction("signup", new { error = "Something went wrong." });
            }
        }

        [HttpGet]
        public async Task<ActionResult> LogOut()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("login", "home");
            }

            await this._signInManager.SignOutAsync();

            return RedirectToAction("index");
        }
    }
}
