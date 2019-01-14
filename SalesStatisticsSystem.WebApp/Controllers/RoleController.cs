using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using SalesStatisticsSystem.WebApp.Models;

namespace SalesStatisticsSystem.WebApp.Controllers
{
    public class RoleController : Controller
    {
        public ApplicationDbContext Context { get; }

        public RoleController()
        {
            Context = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!IsAdminUser())
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            var roles = Context.Roles.ToList();
            return View(roles);
        }

        public bool IsAdminUser()
        {
            if (!User.Identity.IsAuthenticated) return false;

            var user = User.Identity;
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(Context));
            var s = userManager.GetRoles(user.GetUserId());

            return s[0] == "Admin";
        }
    }
}