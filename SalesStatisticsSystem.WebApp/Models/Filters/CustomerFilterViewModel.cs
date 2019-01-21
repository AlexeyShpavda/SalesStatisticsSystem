using SalesStatisticsSystem.WebApp.Models.Filters.Abstract;

namespace SalesStatisticsSystem.WebApp.Models.Filters
{
    public class CustomerFilterViewModel : PagedListParameter
    {
        public string FirstName { get; set; } = null;

        public string LastName { get; set; } = null;
    }
}