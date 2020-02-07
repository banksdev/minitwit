using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Minitwit.API.Models;
using Minitwit.DataAccessLayer;
using Minitwit.Models;
using Minitwit.Website;
using Minitwit.Website.Models;

namespace Minitwit.API.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CustomDbContext _context;

        public HomeController(ILogger<HomeController> logger, CustomDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var vm = new MessagesViewModel()
            {
                Messages = _context.Messages.Select(x => x).ToList()
            };
            if (CookieHandler.LoggedIn(HttpContext.Request))
            {
                var username = HttpContext.Request.Cookies["user"];
                var user = _context.Users.First(u => u.Username == username);
                if (user == null) return RedirectToAction("Index");
                vm.User = user;
            }

            return View(vm);
        }


        [HttpGet]
        public IActionResult Login()
        {
            if (CookieHandler.LoggedIn(HttpContext.Request))
                return RedirectToAction("Index");
            else
                return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel vm)
        {
            if (CookieHandler.LoggedIn(HttpContext.Request)) return RedirectToAction("Index");
            if (!ModelState.IsValid) return View();

            var user = _context.Users.Where(u => u.Username == vm.Username).First();
            if (user == null) return View(); // No user found with that name


            if(PasswordHandler.Validate(vm.Password, user.PasswordHash))
            {
                var options = new CookieOptions();
                options.Expires = DateTime.UtcNow.AddSeconds(60);
                HttpContext.Response.Cookies.Append("user", user.Username, options);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (CookieHandler.LoggedIn(HttpContext.Request))
                return RedirectToAction("Index");
            else
                return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel vm)
        {
            //var k = HttpContext.Session.Keys;
            if (!ModelState.IsValid) return View();
            if (CookieHandler.LoggedIn(HttpContext.Request)) return View("Index");

            var users =_context.Users.Where(u => u.Username == vm.Username).Select(u => u).ToList();
            if (users.Count != 0) return View();

            if(vm.Password == vm.RepeatedPassword)
            {
                var passwordHash = PasswordHandler.CreatePasswordHash(vm.Password);

                var user = new User()
                {
                    Username = vm.Username,
                    Email = vm.Email,
                    PasswordHash = passwordHash              
                };

                _context.Users.Add(user);
                var saved = _context.SaveChanges();

                if(saved > 0)
                {
                    var options = new CookieOptions();
                    options.Expires = DateTime.UtcNow.AddSeconds(60);
                    HttpContext.Response.Cookies.Append("user", user.Username, options);
                }
                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult LogOut()
        {
            HttpContext.Response.Cookies.Delete("user");
            return RedirectToAction("Login");
        }

        [HttpPost]
        public IActionResult AddMessage(string content) 
        {
            if (!CookieHandler.LoggedIn(HttpContext.Request)) return RedirectToAction("Index");
            //if (!ModelState.IsValid) return RedirectToAction("Index");

            var username = HttpContext.Request.Cookies["user"];
            var user = _context.Users.First(u => u.Username == username);
            if (user == null) return RedirectToAction("Index");

            var msg = new Message()
            {
                Content = content,
                Flagged = false,
                PublishedTime = DateTime.UtcNow,
                User = user
            };

            _context.Messages.Add(msg);
            var saved = _context.SaveChanges();

            if (saved > 0) return RedirectToAction("Index");

            return RedirectToAction("Index");

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
