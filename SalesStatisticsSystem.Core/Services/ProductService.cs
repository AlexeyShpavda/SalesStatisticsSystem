using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;
using SalesStatisticsSystem.Contracts.Core.Services;
using SalesStatisticsSystem.Contracts.DataAccessLayer.ReaderWriter;
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

        public async Task<IPagedList<ProductDto>> GetUsingPagedListAsync(int pageNumber, int pageSize,
            Expression<Func<ProductDto, bool>> predicate = null, SortDirection sortDirection = SortDirection.Ascending)
        {
            return await ProductDbReaderWriter.GetUsingPagedListAsync(pageNumber, pageSize, predicate)
                .ConfigureAwait(false);
        }

        public async Task<ProductDto> GetAsync(int id)
        {
            return await ProductDbReaderWriter.GetAsync(id).ConfigureAwait(false);
        }

        public async Task<ProductDto> AddAsync(ProductDto model)
        {
            return await ProductDbReaderWriter.AddAsync(model).ConfigureAwait(false);
        }

        public async Task<ProductDto> UpdateAsync(ProductDto model)
        {
            return await ProductDbReaderWriter.UpdateAsync(model).ConfigureAwait(false);
        }

        public async Task DeleteAsync(int id)
        {
            await ProductDbReaderWriter.DeleteAsync(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<ProductDto>> FindAsync(Expression<Func<ProductDto, bool>> predicate)
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