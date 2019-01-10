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

        public async Task<ManagerDto> AddUniqueManagerToDatabaseAsync(ManagerDto managerDto)
        {
            if (await DoesManagerExistAsync(managerDto)) throw new ArgumentException("Manager already exists!");

            return Add(managerDto);
        }

        public async Task<int> GetIdAsync(string managerLastName)
        {
            Expression<Func<ManagerDto, bool>> predicate = x => x.LastName == managerLastName;

            var result = await Find(predicate);

            return result.First().Id;
        }

        public async Task<bool> DoesManagerExistAsync(ManagerDto managerDto)
        {
            Expression<Func<ManagerDto, bool>> predicate = x => x.LastName == managerDto.LastName;

            var result = await Find(predicate);

            return result.Any();
        }
    }
}