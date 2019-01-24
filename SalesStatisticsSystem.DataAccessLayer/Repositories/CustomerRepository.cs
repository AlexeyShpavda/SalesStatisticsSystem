using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using SalesStatisticsSystem.Core.Contracts.Models.Sales;
using SalesStatisticsSystem.DataAccessLayer.Contracts.Repository;
using SalesStatisticsSystem.DataAccessLayer.Repositories.Abstract;
using SalesStatisticsSystem.Entity;

namespace SalesStatisticsSystem.DataAccessLayer.Repositories
{
    public class CustomerRepository : GenericRepository<CustomerCoreModel, Customer>, ICustomerRepository
    {
        public CustomerRepository(SalesInformationEntities context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<bool> TryAddUniqueCustomerAsync(CustomerCoreModel customerCoreModel)
        {
            if (await DoesCustomerExistAsync(customerCoreModel).ConfigureAwait(false))
            {
                return false;
            }

            Add(customerCoreModel);
            return true;
        }

        public async Task<int> GetIdAsync(string customerFirstName, string customerLastName)
        {
            Expression<Func<CustomerCoreModel, bool>> predicate = x =>
                x.FirstName == customerFirstName && x.LastName == customerLastName;

            var result = await FindAsync(predicate).ConfigureAwait(false);

            return result.First().Id;
        }

        public async Task<bool> DoesCustomerExistAsync(CustomerCoreModel customerCoreModel)
        {
            Expression<Func<CustomerCoreModel, bool>> predicate = x =>
                x.LastName == customerCoreModel.LastName && x.FirstName == customerCoreModel.FirstName;

            var result = await FindAsync(predicate).ConfigureAwait(false);

            return result.Any();
        }
    }
}