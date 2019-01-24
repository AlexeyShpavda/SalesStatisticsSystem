using System.Threading.Tasks;
using SalesStatisticsSystem.Core.Contracts.Models.Sales;
using SalesStatisticsSystem.DataAccessLayer.Contracts.Repository.Generic;

namespace SalesStatisticsSystem.DataAccessLayer.Contracts.Repository
{
    public interface IManagerRepository : IGenericRepository<ManagerCoreModel>
    {
        Task<bool> TryAddUniqueManagerAsync(ManagerCoreModel managerCoreModel);

        Task<int> GetIdAsync(string managerLastName);

        Task<bool> DoesManagerExistAsync(ManagerCoreModel managerCoreModel);
    }
}