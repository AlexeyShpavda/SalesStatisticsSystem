using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects.Abstract;

namespace SalesStatisticsSystem.Contracts.DataAccessLayer.Repositories
{
    public interface IGenericRepository<TDto> where TDto : DataTransferObject
    {
        TDto Add(TDto model);

        TDto Update(TDto model);

        void Remove(int id);

        TDto Get(int id);

        Task<IEnumerable<TDto>> GetAllAsync();

        IEnumerable<TDto> Find(Expression<Func<TDto, bool>> predicate);

        Task<int> SaveAsync();
    }
}