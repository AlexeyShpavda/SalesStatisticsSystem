using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;

namespace SalesStatisticsSystem.Contracts.DataAccessLayer.UnitOfWorks
{
    public interface ICustomerUnitOfWork : IGenericUnitOfWork<CustomerDto>
    { 
    }
}