using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SalesStatisticsSystem.WebApplication.Models.SaleViewModels
{
    public class SaleViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public CustomerViewModel Customer { get; set; }

        public ProductViewModel Product { get; set; }

        [Required]
        [Display(Name = "Sum")]
        public double Sum { get; set; }

        public ManagerViewModel Manager { get; set; }
    }
}