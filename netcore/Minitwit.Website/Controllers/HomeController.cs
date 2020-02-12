using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
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
        private readonly FollowingHelper _followingHelper;

        public HomeController(ILogger<HomeController> logger, CustomDbContext context)
        {
            _logger = logger;
            _context = context;
            _followingHelper = new FollowingHelper(_context);
        }

        public IActionResult Index()
        {
            var res = _context.Messages.Include(x => x.User).ToList();
            var vm = new MessagesViewModel()
            {
                Messages = res,
                FollowingHelper = (userCookie, user) => Following(userCookie, user),
                IsMe = (userCookie, user) => IsMe(userCookie, user)
            };
            if (CookieHandler.LoggedIn(HttpContext.Request))
            {
                var username = HttpContext.Request.Cookies["user"];
                //var user = _context.Users.First(u => u.Username == username);
                //if (user == null) return RedirectToAction("Index");
                vm.User = username;
            }

            return View(vm);
        }

        [HttpGet]
        public IActionResult Private(string username)
        {
            var errors = new List<string>();
            if (!CookieHandler.LoggedIn(HttpContext.Request)) return RedirectToAction("Index");
            var username2 = HttpContext.Request.Cookies["user"];
            var user = _context.Users.First(u => u.Username == username2);
            if (user == null) return RedirectToAction("Index");

            var msg = _context.Messages.
                Join(_context.Followers, 
                m => m.User.UserId, 
                f => f.Self.UserId, 
                (m,f) => new { Message = m, Follower = f}).
                Where(elem => elem.Message.UserId == user.UserId && elem.Follower.Self.UserId == user.UserId)
                .Select(elem => elem.Message).ToList();//.Where(m => m.UserId == user.UserId && Following.self.userid == user.UserId)
            
            var vm = new MessagesViewModel()
            {
                Messages = msg,
                User = HttpContext.Request.Cookies["user"],
                FollowingHelper = (userCookie, user) => Following(userCookie, user),
                IsMe = (userCookie, user) => IsMe(userCookie, user)
            };

            return View(vm);
        }

        private bool IsMe(IRequestCookieCollection userCookie, User author)
        {
            var username = userCookie["user"];
            if (username == "" || username == null) return false;
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            return (user.UserId == author.UserId);
        }


        private bool Following(IRequestCookieCollection userCookie, User author)
        {
            var username = userCookie["user"];
            if (username == "" || username == null) return false;
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            return !_context.Followers.Any(f => f.Self.UserId == user.UserId && f.Following.UserId == author.UserId);
        }

        [HttpGet]
        public IActionResult Login()
        {
            var c = HttpContext.Request.Cookies;
            if (CookieHandler.LoggedIn(HttpContext.Request))
                return RedirectToAction("Index");
            else
                return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel vm)
        {
            var errors = new List<string>();
            if (CookieHandler.LoggedIn(HttpContext.Request)) return RedirectToAction("Index");
            if (!ModelState.IsValid)
            {
                errors.Add("Something went wrong");
                ViewData["Errors"] = errors;
                return View();
            }
            User user;
            try
            {
                user = _context.Users.Where(u => u.Username == vm.Username).First();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                errors.Add("This user does not exist");
                ViewData["Errors"] = errors;
                return View();
            }


            if (PasswordHandler.Validate(vm.Password, user.PasswordHash))
            {
                var options = new CookieOptions();
                options.Expires = DateTime.UtcNow.AddSeconds(60);
                HttpContext.Response.Cookies.Append("user", user.Username, options);
            }
            else
            {
                errors.Add("Wrong username/password");
                ViewData["Errors"] = errors;
                return View();
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
            if (!ModelState.IsValid) return View();
            if (CookieHandler.LoggedIn(HttpContext.Request)) return View("Index");
            var errors = new List<string>();
            var users =_context.Users.Select(u => u.Username == vm.Username).ToList();
            if (users.Count != 0)
            {
                errors.Add("This user already exists");
                ViewData["Errors"] = errors;
                return View();
            }

            if (vm.Password == vm.RepeatedPassword)
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
                }else
                {
                    errors.Add("Something went wrong, contact the administrator!");
                    ViewData["Errors"] = errors;
                    return View();
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
                User = user,
                UserId = user.UserId
            };

            _context.Messages.Add(msg);
            var saved = _context.SaveChanges();

            if (saved > 0) return RedirectToAction("Index");

            return RedirectToAction("Index");

        }

        [HttpPost]
        public IActionResult Follow(Guid personToFollow)
        {
            if (!CookieHandler.LoggedIn(HttpContext.Request)) return RedirectToAction("Index");
            var username = HttpContext.Request.Cookies["user"];
            var user = _context.Users.First(u => u.Username == username);
            if (user.UserId == personToFollow) return RedirectToAction("Index");
            var following = _context.Followers.Where(f => f.Self.UserId == user.UserId && f.Following.UserId == personToFollow).ToList();
            if(following.Count == 0)
            {
                var followingPerson = _context.Users.FirstOrDefault(u => u.UserId == personToFollow);
                var follower = new Follower()
                {
                    Self = user,
                    Following = followingPerson
                };
                _context.Followers.Add(follower);
                var changed = _context.SaveChanges();
                if (changed == 1) return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
