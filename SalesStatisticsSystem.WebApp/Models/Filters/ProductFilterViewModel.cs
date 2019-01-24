using SalesStatisticsSystem.WebApp.Models.Filters.Abstract;

namespace SalesStatisticsSystem.WebApp.Models.Filters
{
    public class ProductFilterViewModel : PagedListParameterViewModel
    {
        public string Name { get; set; } = null;
    }
}