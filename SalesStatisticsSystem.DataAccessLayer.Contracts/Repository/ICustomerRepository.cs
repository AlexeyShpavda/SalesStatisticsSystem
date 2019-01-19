using System.Threading.Tasks;
using SalesStatisticsSystem.Core.Contracts.Models;
using SalesStatisticsSystem.DataAccessLayer.Contracts.Repository.Generic;

namespace SalesStatisticsSystem.DataAccessLayer.Contracts.Repository
{
    public interface ICustomerRepository : IGenericRepository<CustomerCoreModel>
    {
        Task<bool> TryAddUniqueCustomerAsync(CustomerCoreModel customerCoreModel);

        Task<int> GetIdAsync(string customerFirstName, string customerLastName);

        Task<bool> DoesCustomerExistAsync(CustomerCoreModel customerCoreModel);
    }
}