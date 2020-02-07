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
        public User User { get; set; }
    }
}
