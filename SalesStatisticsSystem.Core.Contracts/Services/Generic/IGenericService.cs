using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Helpers;
using SalesStatisticsSystem.Core.Contracts.Models.Filters.Abstract;
using SalesStatisticsSystem.Core.Contracts.Models.Sales.Abstract;
using X.PagedList;

namespace SalesStatisticsSystem.Core.Contracts.Services.Generic
{
    public interface IGenericService<TCoreModel, in TFilterCoreModel>
        where TCoreModel : CoreModel
        where TFilterCoreModel : PagedListParameterCoreModel
    {
        Task<IPagedList<TCoreModel>> GetUsingPagedListAsync(int pageNumber, int pageSize,
            Expression<Func<TCoreModel, bool>> predicate = null, SortDirection sortDirection = SortDirection.Ascending);

        Task<IPagedList<TCoreModel>> Filter(TFilterCoreModel filterCoreModel, int pageSize,
            SortDirection sortDirection = SortDirection.Ascending);

        Task<TCoreModel> GetAsync(int id);

        Task<TCoreModel> AddAsync(TCoreModel model);

        Task<TCoreModel> UpdateAsync(TCoreModel model);

        Task DeleteAsync(int id);

        Task<IEnumerable<TCoreModel>> FindAsync(Expression<Func<TCoreModel, bool>> predicate);
    }
}