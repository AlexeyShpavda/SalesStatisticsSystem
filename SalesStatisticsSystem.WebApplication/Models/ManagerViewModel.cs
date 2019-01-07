using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SalesStatisticsSystem.WebApplication.Models
{
    public class ManagerViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Last Name length must be less than 50 characters")]
        public string LastName { get; set; }
    }
}