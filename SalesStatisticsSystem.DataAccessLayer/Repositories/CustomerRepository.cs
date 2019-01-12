using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;
using SalesStatisticsSystem.Contracts.DataAccessLayer.Repositories;
using SalesStatisticsSystem.DataAccessLayer.Repositories.Abstract;
using SalesStatisticsSystem.Entity;

namespace SalesStatisticsSystem.DataAccessLayer.Repositories
{
    public class CustomerRepository : GenericRepository<CustomerDto, Customer>, ICustomerRepository
    {
        public CustomerRepository(SalesInformationEntities context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<bool> TryAddUniqueCustomerAsync(CustomerDto customerDto)
        {
            if (await DoesCustomerExistAsync(customerDto).ConfigureAwait(false))
            {
                return false;
            }

            Add(customerDto);
            return true;
        }

        public async Task<int> GetIdAsync(string customerFirstName, string customerLastName)
        {
            Expression<Func<CustomerDto, bool>> predicate = x =>
                x.FirstName == customerFirstName && x.LastName == customerLastName;

            var result = await FindAsync(predicate).ConfigureAwait(false);

            return result.First().Id;
        }

        public async Task<bool> DoesCustomerExistAsync(CustomerDto customerDto)
        {
            Expression<Func<CustomerDto, bool>> predicate = x =>
                x.LastName == customerDto.LastName && x.FirstName == customerDto.FirstName;

            var result = await FindAsync(predicate).ConfigureAwait(false);

            return result.Any();
        }
    }
}