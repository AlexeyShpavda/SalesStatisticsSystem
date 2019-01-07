using System.Data.Entity;
using SalesStatisticsSystem.WebApplication.Models.Authentication.Models;

namespace SalesStatisticsSystem.WebApplication.Models.Authentication
{
    public class UserDbInitializer : DropCreateDatabaseAlways<UserContext>
    {
        protected override void Seed(UserContext userContext)
        {
            userContext.Roles.Add(new Role {Id = 1, Name = "admin"});
            userContext.Roles.Add(new Role {Id = 2, Name = "user"});

            userContext.Users.Add(new User
            {
                Id = 1,
                Email = "as@gmail.com",
                Password = "123",
                RoleId = 1
            });

            base.Seed(userContext);
        }
    }
}