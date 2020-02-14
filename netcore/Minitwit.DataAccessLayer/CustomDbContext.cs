using Microsoft.EntityFrameworkCore;
using Minitwit.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Minitwit.DataAccessLayer
{
    public class CustomDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Follower> Followers { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Mini;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;"
            var connection = @"Data Source=localhost,11433 ;Initial Catalog=MyDB;User ID=sa;Password=p@55w0rd;";
            options.UseSqlServer(connection);
        }
    }
}
