using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using SalesStatisticsSystem.Core.Contracts.Models;
using SalesStatisticsSystem.DataAccessLayer.Contracts.Repository;
using SalesStatisticsSystem.DataAccessLayer.Repositories.Abstract;
using SalesStatisticsSystem.Entity;

namespace SalesStatisticsSystem.DataAccessLayer.Repositories
{
    public class ManagerRepository : GenericRepository<ManagerCoreModel, Manager>, IManagerRepository
    {
        public ManagerRepository(SalesInformationEntities context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<bool> TryAddUniqueManagerAsync(ManagerCoreModel managerDto)
        {
            if (await DoesManagerExistAsync(managerDto).ConfigureAwait(false))
            {
                return false;
            }

            Add(managerDto);
            return true;
        }

        public async Task<int> GetIdAsync(string managerLastName)
        {
            Expression<Func<ManagerCoreModel, bool>> predicate = x => x.LastName == managerLastName;

            var result = await FindAsync(predicate).ConfigureAwait(false);

            return result.First().Id;
        }

        public async Task<bool> DoesManagerExistAsync(ManagerCoreModel managerDto)
        {
            Expression<Func<ManagerCoreModel, bool>> predicate = x => x.LastName == managerDto.LastName;

            var result = await FindAsync(predicate).ConfigureAwait(false);

            return result.Any();
        }
    }
}