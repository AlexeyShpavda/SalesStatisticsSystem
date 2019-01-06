using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects.Abstract;

namespace SalesStatisticsSystem.Contracts.DataAccessLayer.Repositories
{
    public interface IGenericRepository<TDto> where TDto : DataTransferObject
    {
        void Add(params TDto[] models);

        void Update(params TDto[] entities);

        void Remove(params TDto[] entities);

        Task<TDto> Get(int id);

        Task<IEnumerable<TDto>> GetAllAsync();

        IEnumerable<TDto> Find(Expression<Func<TDto, bool>> predicate);

        void Save();
    }
}