using System.Collections.Generic;
using System.Threading.Tasks;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;

namespace SalesStatisticsSystem.Contracts.DataAccessLayer.UnitOfWorks
{
    public interface ISaleUnitOfWork
    {
        void Add(params SaleDto[] models);

        Task<IEnumerable<SaleDto>> GetSalesAsync();
    }
}