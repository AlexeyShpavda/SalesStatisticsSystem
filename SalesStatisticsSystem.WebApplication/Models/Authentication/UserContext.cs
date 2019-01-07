using System.Data.Entity;

namespace SalesStatisticsSystem.WebApplication.Models.Authentication
{
    public class UserContext : DbContext
    {
        public UserContext() : base("UserContext")
        {
        }

        public DbSet<User> Users { get; set; }
    }
}