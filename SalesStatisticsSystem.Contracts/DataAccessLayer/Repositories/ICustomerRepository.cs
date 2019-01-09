using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;

namespace SalesStatisticsSystem.Contracts.DataAccessLayer.Repositories
{
    public interface ICustomerRepository : IGenericRepository<CustomerDto>
    {
        CustomerDto AddUniqueCustomerToDatabase(CustomerDto customerDto);

        int? GetId(string customerFirstName, string customerLastName);

        bool DoesCustomerExist(CustomerDto customerDto);
    }
}