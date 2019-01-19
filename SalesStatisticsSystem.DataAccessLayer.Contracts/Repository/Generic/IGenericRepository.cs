using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Helpers;
using SalesStatisticsSystem.Core.Contracts.Models.Abstract;
using X.PagedList;

namespace SalesStatisticsSystem.DataAccessLayer.Contracts.Repository.Generic
{
    public interface IGenericRepository<TCoreModel> where TCoreModel : CoreModel
    {
        TCoreModel Add(TCoreModel model);

        TCoreModel Update(TCoreModel model);

        Task DeleteAsync(int id);

        Task<TCoreModel> GetAsync(int id);

        Task<IPagedList<TCoreModel>> GetUsingPagedListAsync(int pageNumber, int pageSize,
            Expression<Func<TCoreModel, bool>> predicate = null, SortDirection sortDirection = SortDirection.Ascending);

        Task<IEnumerable<TCoreModel>> FindAsync(Expression<Func<TCoreModel, bool>> predicate);

        Task<int> SaveAsync();
    }
}