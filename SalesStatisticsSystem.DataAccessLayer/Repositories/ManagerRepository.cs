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
    public class ManagerRepository : GenericRepository<ManagerDto, Manager>, IManagerRepository
    {
        public ManagerRepository(SalesInformationEntities context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<bool> TryAddUniqueManagerAsync(ManagerDto managerDto)
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
            Expression<Func<ManagerDto, bool>> predicate = x => x.LastName == managerLastName;

            var result = await FindAsync(predicate).ConfigureAwait(false);

            return result.First().Id;
        }

        public async Task<bool> DoesManagerExistAsync(ManagerDto managerDto)
        {
            Expression<Func<ManagerDto, bool>> predicate = x => x.LastName == managerDto.LastName;

            var result = await FindAsync(predicate).ConfigureAwait(false);

            return result.Any();
        }
    }
}