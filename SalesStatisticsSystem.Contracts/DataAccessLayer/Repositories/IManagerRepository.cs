using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;

namespace SalesStatisticsSystem.Contracts.DataAccessLayer.Repositories
{
    public interface IManagerRepository : IGenericRepository<ManagerDto>
    {
        ManagerDto AddUniqueManagerToDatabase(ManagerDto managerDto);

        int? GetId(string managerLastName);

        bool DoesManagerExist(ManagerDto managerDto);
    }
}