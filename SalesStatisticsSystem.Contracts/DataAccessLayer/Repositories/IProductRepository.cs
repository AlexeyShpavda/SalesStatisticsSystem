using System.Threading.Tasks;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;

namespace SalesStatisticsSystem.Contracts.DataAccessLayer.Repositories
{
    public interface IProductRepository : IGenericRepository<ProductDto>
    {
        Task<bool> TryAddUniqueProductAsync(ProductDto productDto);

        Task<int> GetIdAsync(string productName);

        Task<bool> DoesProductExistAsync(ProductDto productDto);
    }
}