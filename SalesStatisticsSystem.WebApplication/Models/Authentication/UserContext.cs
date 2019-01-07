using SalesStatisticsSystem.WebApplication.Models.Authentication.Models;

namespace SalesStatisticsSystem.WebApplication.Models.Authentication
{
    using System.Data.Entity;

    public class UserContext : DbContext
    {
        public UserContext() : base("name=UserContext")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}