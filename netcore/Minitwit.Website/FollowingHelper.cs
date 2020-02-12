using Microsoft.AspNetCore.Http;
using Minitwit.DataAccessLayer;
using Minitwit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minitwit.Website
{
    public class FollowingHelper
    {
        private readonly CustomDbContext _context;

        public FollowingHelper(CustomDbContext context)
        {
            _context = context;
        }

       
    }
}
