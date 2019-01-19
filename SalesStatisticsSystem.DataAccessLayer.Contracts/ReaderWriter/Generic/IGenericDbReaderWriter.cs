using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Helpers;
using SalesStatisticsSystem.Core.Contracts.Models.Abstract;
using X.PagedList;

namespace SalesStatisticsSystem.DataAccessLayer.Contracts.ReaderWriter.Generic
{
    public interface IGenericDbReaderWriter<TCoreModel> where TCoreModel : CoreModel
    {
        Task<TCoreModel> AddAsync(TCoreModel model);

        Task<TCoreModel> UpdateAsync(TCoreModel model);

        Task DeleteAsync(int id);

        Task<IPagedList<TCoreModel>> GetUsingPagedListAsync(int pageNumber, int pageSize,
            Expression<Func<TCoreModel, bool>> predicate = null, SortDirection sortDirection = SortDirection.Ascending);

        Task<TCoreModel> GetAsync(int id);

        Task<IEnumerable<TCoreModel>> FindAsync(Expression<Func<TCoreModel, bool>> predicate);
    }
}