using System;
using System.Collections.Generic;
using System.Text;

namespace Minitwit.Models
{
    public class Message
    {
        public Guid MessageId { get; set; }
        public string Content { get; set; }
        public DateTime PublishedTime { get; set; }
        public bool Flagged { get; set; }
        public User User { get; set; }
        public Guid UserId { get; set; }
    }
}
