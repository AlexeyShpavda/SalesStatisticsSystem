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

        Task DeleteAsync(int id);

        Task<TDto> GetAsync(int id);

        Task<IEnumerable<TDto>> GetAllAsync();

        Task<IEnumerable<TDto>> Find(Expression<Func<TDto, bool>> predicate);

        Task<int> SaveAsync();
    }
}