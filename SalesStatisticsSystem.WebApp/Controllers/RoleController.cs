using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SalesStatisticsSystem.WebApp.Models;
using ActionResult = Microsoft.AspNetCore.Mvc.ActionResult;
using Controller = Microsoft.AspNetCore.Mvc.Controller;

namespace SalesStatisticsSystem.WebApp.Controllers
{
    public class RoleController : Controller
    {
        public ApplicationDbContext Context { get; }

        public RoleController()
        {
            Context = new ApplicationDbContext();
        }

        [HttpGet]
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