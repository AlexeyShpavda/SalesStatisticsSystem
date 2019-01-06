using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;

namespace SalesStatisticsSystem.Contracts.DataAccessLayer.Repositories
{
    public interface IProductRepository : IGenericRepository<ProductDto>
    {
        void AddUniqueProductToDatabase(ProductDto productDto);

        int? GetId(string productName);

        bool DoesProductExist(ProductDto productDto);
    }
}