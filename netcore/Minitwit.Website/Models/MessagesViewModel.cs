using Microsoft.AspNetCore.Http;
using Minitwit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minitwit.Website.Models
{
    public class MessagesViewModel
    {
        public List<Message> Messages { get; set; }
        public string User { get; set; }
        public Func<IRequestCookieCollection, User, bool> FollowingHelper { get; set; }
        public Func<IRequestCookieCollection, User, bool> IsMe { get; set; }

    }
}
