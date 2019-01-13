using SalesStatisticsSystem.WebApp.Models.Filters.Abstract;

namespace SalesStatisticsSystem.WebApp.Models.Filters
{
    public class CustomerFilterModel : PagedListParameter
    {
        public string FirstName { get; set; } = null;

        public string LastName { get; set; } = null;
    }
}