using System.ComponentModel.DataAnnotations;

namespace SalesStatisticsSystem.WebApplication.Models.Authentication.ViewModels
{
    public class LoginModel
    {
        [Display(Name = "Email")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}