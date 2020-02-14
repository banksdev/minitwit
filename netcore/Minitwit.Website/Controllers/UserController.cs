using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Minitwit.API.Models;

namespace Minitwit.API.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public UserController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Timeline(string username)
        {
            throw new NotImplementedException();
        }


        public IActionResult Follow(string username)
        {
            throw new NotImplementedException();
        }

        public IActionResult UnFollow(string username)
        {
            throw new NotImplementedException();
        }

        //[HttpPost]
        //public IActionResult Post(MessageViewModel message)
        //{
        //    throw new NotImplementedException();
        //}

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public IActionResult Login()
        {
            throw new NotImplementedException();
        }

        //[HttpPost]
        //public IActionResult Register(UserViewModel user)
        //{
        //    throw new NotImplementedException();
        //}

        [HttpGet]
        public IActionResult Register()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            throw new NotImplementedException();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
