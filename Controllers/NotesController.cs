using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Data;
using Models;

namespace private_notes.Controllers
{
    public class NotesController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public NotesController(DatabaseContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this._context = context;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            // redirect to /home/login if not authenticated 
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("login", "home");
            }

            User user = (User)await this._userManager.FindByNameAsync(User.Identity.Name);
            // explicit loading all notes from single user 
            this._context.Entry(user).Collection(u => u.Notes).Load();
            ViewData["Username"] = user.UserName;

            if (user.Notes.Any())
            {
                ViewData["Notes"] = user.Notes;
            }

            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("login", "home");
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(string title, string description)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("login", "home");
            }

            User user = (User)await this._userManager.FindByNameAsync(User.Identity.Name);
            this._context.Entry(user).Collection(u => u.Notes).Load();

            user.Notes.Add(new Note { Title = title, Content = description });
            await this._context.SaveChangesAsync();

            return RedirectToAction("index");
        }

        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("login", "home");
            }

            User user = (User)await this._userManager.FindByNameAsync(User.Identity.Name);
            this._context.Entry(user).Collection(u => u.Notes).Load();
            Note note = user.Notes.Where(n => n.Id == id).SingleOrDefault();

            if (note != null)
            {
                ViewData["Note"] = note;
                return View();
            }

            return RedirectToAction("index");
        }

        [HttpPost]
        public async Task<ActionResult> Edit(string identifier, string title, string description)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("login", "home");
            }

            User user = (User)await this._userManager.FindByNameAsync(User.Identity.Name);
            this._context.Entry(user).Collection(u => u.Notes).Load();
            Note note = user.Notes.Where(n => n.Id == identifier).SingleOrDefault();

            if (note != null)
            {
                note.Title = title;
                note.Content = description;
                await this._context.SaveChangesAsync();
            }

            return RedirectToAction("index");
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("login", "home");
            }

            User user = (User)await this._userManager.FindByNameAsync(User.Identity.Name);
            this._context.Entry(user).Collection(u => u.Notes).Load();
            Note note = user.Notes.Where(n => n.Id == id).SingleOrDefault();

            if (note != null)
            {
                this._context._Notes.Remove(note);
                await this._context.SaveChangesAsync();
            }

            return RedirectToAction("index");
        }
    }
}
