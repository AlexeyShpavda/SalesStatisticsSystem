using System.Threading.Tasks;
using SalesStatisticsSystem.Core.Contracts.Models.Sales;
using SalesStatisticsSystem.DataAccessLayer.Contracts.Repository.Generic;

namespace SalesStatisticsSystem.DataAccessLayer.Contracts.Repository
{
    public interface IProductRepository : IGenericRepository<ProductCoreModel>
    {
        Task<bool> TryAddUniqueProductAsync(ProductCoreModel productCoreModel);

        Task<int> GetIdAsync(string productName);

        Task<bool> DoesProductExistAsync(ProductCoreModel productCoreModel);
    }
}