using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Helpers;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects.Abstract;
using X.PagedList;

namespace SalesStatisticsSystem.Contracts.DataAccessLayer.ReaderWriter
{
    public interface IGenericDbReaderWriter<TDto> where TDto : DataTransferObject
    {
        Task<TDto> AddAsync(TDto model);

        Task<TDto> UpdateAsync(TDto model);

        Task DeleteAsync(int id);

        Task<IPagedList<TDto>> GetUsingPagedListAsync(int pageNumber, int pageSize,
            Expression<Func<TDto, bool>> predicate = null, SortDirection sortDirection = SortDirection.Ascending);

        Task<TDto> GetAsync(int id);

        Task<IEnumerable<TDto>> FindAsync(Expression<Func<TDto, bool>> predicate);
    }
}