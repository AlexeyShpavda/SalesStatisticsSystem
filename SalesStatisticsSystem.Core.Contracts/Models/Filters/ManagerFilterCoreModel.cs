using SalesStatisticsSystem.Core.Contracts.Models.Filters.Abstract;

namespace SalesStatisticsSystem.Core.Contracts.Models.Filters
{
    public abstract class ManagerFilterCoreModel : PagedListParameterCoreModel
    {
        public string LastName { get; set; } = null;
    }
}