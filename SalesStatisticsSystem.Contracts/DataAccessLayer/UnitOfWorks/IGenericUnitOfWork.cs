using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects.Abstract;

namespace SalesStatisticsSystem.Contracts.DataAccessLayer.UnitOfWorks
{
    public interface IGenericUnitOfWork<TDto> where TDto :DataTransferObject
    {
        Task<TDto> AddAsync(TDto model);

        Task<TDto> UpdateAsync(TDto model);

        Task DeleteAsync(int id);

        Task<IEnumerable<TDto>> GetAllAsync();

        Task<TDto> GetAsync(int id);

        Task<IEnumerable<TDto>> FindAsync(Expression<Func<TDto, bool>> predicate);
    }
}