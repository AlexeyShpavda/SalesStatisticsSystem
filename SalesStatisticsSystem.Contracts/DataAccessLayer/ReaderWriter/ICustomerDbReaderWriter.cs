using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;

namespace SalesStatisticsSystem.Contracts.DataAccessLayer.ReaderWriter
{
    public interface ICustomerDbReaderWriter : IGenericDbReaderWriter<CustomerDto>
    { 
    }
}