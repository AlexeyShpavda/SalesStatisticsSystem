using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;
using SalesStatisticsSystem.Core.Contracts.Models.Filters;
using SalesStatisticsSystem.Core.Contracts.Models.Sales;
using SalesStatisticsSystem.Core.Contracts.Services;
using SalesStatisticsSystem.DataAccessLayer.Contracts.ReaderWriter;
using SalesStatisticsSystem.DataAccessLayer.ReaderWriter;
using SalesStatisticsSystem.Entity;
using X.PagedList;

namespace SalesStatisticsSystem.Core.Services
{
    public class ProductService : IProductService, IDisposable
    {
        private SalesInformationEntities Context { get; }

        private ReaderWriterLockSlim Locker { get; }

        private IProductDbReaderWriter ProductDbReaderWriter { get; }

        public ProductService()
        {
            Context = new SalesInformationEntities();

            Locker = new ReaderWriterLockSlim();

            ProductDbReaderWriter = new ProductDbReaderWriter(Context, Locker);
        }

        public async Task<IPagedList<ProductCoreModel>> GetUsingPagedListAsync(int pageNumber, int pageSize,
            Expression<Func<ProductCoreModel, bool>> predicate = null, SortDirection sortDirection = SortDirection.Ascending)
        {
            return await ProductDbReaderWriter.GetUsingPagedListAsync(pageNumber, pageSize, predicate)
                .ConfigureAwait(false);
        }

        public async Task<IPagedList<ProductCoreModel>> Filter(ProductFilterCoreModel productFilterCoreModel,
            int pageSize, SortDirection sortDirection = SortDirection.Ascending)
        {
            if (productFilterCoreModel.Name == null)
            {
                return await GetUsingPagedListAsync(productFilterCoreModel.Page ?? 1, pageSize)
                    .ConfigureAwait(false);
            }

            return await GetUsingPagedListAsync(productFilterCoreModel.Page ?? 1,
                    pageSize, x => x.Name.Contains(productFilterCoreModel.Name))
                .ConfigureAwait(false);
        }

        public async Task<ProductCoreModel> GetAsync(int id)
        {
            return await ProductDbReaderWriter.GetAsync(id).ConfigureAwait(false);
        }

        public async Task<ProductCoreModel> AddAsync(ProductCoreModel model)
        {
            return await ProductDbReaderWriter.AddAsync(model).ConfigureAwait(false);
        }

        public async Task<ProductCoreModel> UpdateAsync(ProductCoreModel model)
        {
            return await ProductDbReaderWriter.UpdateAsync(model).ConfigureAwait(false);
        }

        public async Task DeleteAsync(int id)
        {
            await ProductDbReaderWriter.DeleteAsync(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<ProductCoreModel>> FindAsync(Expression<Func<ProductCoreModel, bool>> predicate)
        {
            return await ProductDbReaderWriter.FindAsync(predicate).ConfigureAwait(false);
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Locker.Dispose();
                    Context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}