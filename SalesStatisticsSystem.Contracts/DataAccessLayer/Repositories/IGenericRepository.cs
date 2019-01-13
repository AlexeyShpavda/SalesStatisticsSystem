using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Helpers;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects.Abstract;
using X.PagedList;

namespace SalesStatisticsSystem.Contracts.DataAccessLayer.Repositories
{
    public interface IGenericRepository<TDto> where TDto : DataTransferObject
    {
        TDto Add(TDto model);

        TDto Update(TDto model);

        Task DeleteAsync(int id);

        Task<TDto> GetAsync(int id);

        Task<IPagedList<TDto>> GetUsingPagedListAsync(int pageNumber, int pageSize,
            Expression<Func<TDto, bool>> predicate = null, SortDirection sortDirection = SortDirection.Ascending);

        Task<IEnumerable<TDto>> FindAsync(Expression<Func<TDto, bool>> predicate);

        Task<int> SaveAsync();
    }
}