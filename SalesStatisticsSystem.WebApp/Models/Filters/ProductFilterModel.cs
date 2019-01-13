using SalesStatisticsSystem.WebApp.Models.Filters.Abstract;

namespace SalesStatisticsSystem.WebApp.Models.Filters
{
    public class ProductFilterModel : PagedListParameter
    {
        public string Name { get; set; } = null;
    }
}