using Microsoft.AspNetCore.Mvc;
using Minitwit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minitwit.Website.Models
{
    public class RegisterViewModel
    {
        [BindProperty]
        public string Username{ get; set; }
        
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Password { get; set; }
        [BindProperty]
        public string RepeatedPassword { get; set; }
    }
}
