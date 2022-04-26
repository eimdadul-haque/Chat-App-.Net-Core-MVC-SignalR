using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;

namespace Chat_App_.Net_Core_MVC_SignalR.Controllers
{
    public class HomeController : Controller
    {
        private AppDbContext _context;
        private UserManager<UserModel> _userManager;
        private SignInManager<UserModel> _signInManager;
        public HomeController(AppDbContext context, UserManager<UserModel> userManager, SignInManager<UserModel> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel login)
        {
            if (ModelState.IsValid)
            {
                var res = await _signInManager.PasswordSignInAsync(login.email, login.password, false, false);
                if (res.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignInModel signin)
        {
            if (ModelState.IsValid)
            {
                var user = new UserModel
                {
                    name = signin.name,
                    Email = signin.email,
                    UserName = signin.email
                };

                var res = await _userManager.CreateAsync(user, signin.password);
                if (res.Succeeded)
                {
                    return RedirectToAction(nameof(Login));
                }
            }
            return View(signin);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            var res = _signInManager.SignOutAsync();
            if (res.IsCompletedSuccessfully)
            {
                return RedirectToAction(nameof(Login));
            }

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetMessage([FromQuery] string sender, string reciver)
        {
            if (ModelState.IsValid)
            {
                return Ok(await _context.MessageD.Where(x => x.sender.Contains(sender) || x.sender.Contains(reciver) && x.reciver.Contains(sender) || x.reciver.Contains(reciver)).ToListAsync());
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> SaveMessage(MessageModel message)
        {
            if (ModelState.IsValid)
            {
                await _context.MessageD.AddAsync(message);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NoContent();
        }

    }
}