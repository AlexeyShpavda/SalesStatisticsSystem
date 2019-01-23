using SalesStatisticsSystem.Core.Contracts.Models.Filters.Abstract;

namespace SalesStatisticsSystem.Core.Contracts.Models.Filters
{
    public class ProductFilterCoreModel : PagedListParameterCoreModel
    {
        public string Name { get; set; } = null;
    }
}