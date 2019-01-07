using System.Data.Entity;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SalesStatisticsSystem.WebApplication.Models.Authentication;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace SalesStatisticsSystem.WebApplication.Controllers
{
    public class AuthenticationController : Controller
    {
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginModel);
            }

            using (var userContext = new UserContext())
            {
                var user = await userContext.Users.FirstOrDefaultAsync(u =>
                    u.Email == loginModel.Email && u.Password == loginModel.Password);

                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.Email, true);
                    return RedirectToAction("index", "Home");
                }

                ModelState.AddModelError("", "Wrong Login or Password");
            }

            return View(loginModel);
        }

        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Registration(RegistrationModel registrationModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registrationModel);
            }

            using (var userContext = new UserContext())
            {
                var user = await userContext.Users.FirstOrDefaultAsync(u => u.Email == registrationModel.Email);

                if (user == null)
                {
                    userContext.Users.Add(new User()
                        { Email = registrationModel.Email, Password = registrationModel.Password });

                    await userContext.SaveChangesAsync();

                    user = await userContext.Users.FirstOrDefaultAsync(u =>
                        u.Email == registrationModel.Email && u.Password == registrationModel.Password);

                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(user.Email, true);
                        return RedirectToAction("index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("","User is already registered");
                }
            }

            return View(registrationModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

    }
}