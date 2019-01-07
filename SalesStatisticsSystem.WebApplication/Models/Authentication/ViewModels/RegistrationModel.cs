using System.ComponentModel.DataAnnotations;

namespace SalesStatisticsSystem.WebApplication.Models.Authentication.ViewModels
{
    public class RegistrationModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Enter valid email address")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Password must not exceed 50 characters")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}