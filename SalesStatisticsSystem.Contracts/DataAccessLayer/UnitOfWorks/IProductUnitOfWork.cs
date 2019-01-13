using System.Threading.Tasks;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;
using X.PagedList;

namespace SalesStatisticsSystem.Contracts.DataAccessLayer.UnitOfWorks
{
    public interface IProductUnitOfWork : IGenericUnitOfWork<ProductDto>
    {
        Task<IPagedList<ProductDto>> GetUsingPagedListAsync(int number, int size);
    }
}