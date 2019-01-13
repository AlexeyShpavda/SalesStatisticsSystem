using SalesStatisticsSystem.WebApp.Models.Filters.Abstract;

namespace SalesStatisticsSystem.WebApp.Models.Filters
{
    public class ManagerFilterModel : PagedListParameter
    {
        public string LastName { get; set; } = null;
    }
}