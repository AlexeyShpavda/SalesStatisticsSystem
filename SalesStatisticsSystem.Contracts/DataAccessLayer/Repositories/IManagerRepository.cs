using System.Threading.Tasks;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;

namespace SalesStatisticsSystem.Contracts.DataAccessLayer.Repositories
{
    public interface IManagerRepository : IGenericRepository<ManagerDto>
    {
        Task<ManagerDto> AddUniqueManagerToDatabaseAsync(ManagerDto managerDto);

        Task<int> GetIdAsync(string managerLastName);

        Task<bool> DoesManagerExistAsync(ManagerDto managerDto);
    }
}