using SalesStatisticsSystem.Core.Contracts.Models.Filters;
using SalesStatisticsSystem.Core.Contracts.Models.Sales;
using SalesStatisticsSystem.Core.Contracts.Services.Generic;

namespace SalesStatisticsSystem.Core.Contracts.Services
{
    public interface IProductService : IGenericService<ProductCoreModel, ProductFilterCoreModel>
    {
    }
}