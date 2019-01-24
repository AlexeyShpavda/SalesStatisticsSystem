using SalesStatisticsSystem.Core.Contracts.Models.Sales;
using SalesStatisticsSystem.DataAccessLayer.Contracts.Repository.Generic;

namespace SalesStatisticsSystem.DataAccessLayer.Contracts.Repository
{
    public interface ISaleRepository : IGenericRepository<SaleCoreModel>
    {
    }
}