﻿using System.Threading.Tasks;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;

namespace SalesStatisticsSystem.Contracts.DataAccessLayer.Repositories
{
    public interface ICustomerRepository : IGenericRepository<CustomerDto>
    {
        Task<bool> TryAddUniqueCustomerAsync(CustomerDto customerDto);

        Task<int> GetIdAsync(string customerFirstName, string customerLastName);

        Task<bool> DoesCustomerExistAsync(CustomerDto customerDto);
    }
}