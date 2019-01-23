using SalesStatisticsSystem.Core.Contracts.Models.Filters.Abstract;

namespace SalesStatisticsSystem.Core.Contracts.Models.Filters
{
    public class CustomerFilterCoreModel : PagedListParameterCoreModel
    {
        public string FirstName { get; set; } = null;

        public string LastName { get; set; } = null;
    }
}