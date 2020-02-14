using System;

namespace Minitwit.Models
{
    public class Follower
    {
        public Guid FollowerId { get; set; }
        public User Self { get; set; }
        public User Following { get; set; }
    }
}
