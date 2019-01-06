using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;

namespace SalesStatisticsSystem.Contracts.DataAccessLayer.Repositories
{
    public interface ICustomerRepository : IGenericRepository<CustomerDto>
    {
        void AddUniqueCustomerToDatabase(CustomerDto customerDto);

        int? GetId(string customerFirstName, string customerLastName);

        bool DoesCustomerExist(CustomerDto customerDto);
    }
}