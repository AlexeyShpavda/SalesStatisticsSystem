using SalesStatisticsSystem.WebApp.Models.Filters.Abstract;

namespace SalesStatisticsSystem.WebApp.Models.Filters
{
    public class ProductFilterViewModel : PagedListParameter
    {
        public string Name { get; set; } = null;
    }
}