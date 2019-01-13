using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;
using SalesStatisticsSystem.Contracts.Core.Services;
using SalesStatisticsSystem.Contracts.DataAccessLayer.UnitOfWorks;
using SalesStatisticsSystem.DataAccessLayer.UnitOfWorks;
using SalesStatisticsSystem.Entity;
using X.PagedList;

namespace SalesStatisticsSystem.Core.Services
{
    public class ProductService : IProductService, IDisposable
    {
        private SalesInformationEntities Context { get; }

        private ReaderWriterLockSlim Locker { get; }

        private IProductUnitOfWork ProductUnitOfWork { get; }

        public ProductService()
        {
            Context = new SalesInformationEntities();

            Locker = new ReaderWriterLockSlim();

            ProductUnitOfWork = new ProductUnitOfWork(Context, Locker);
        }

        public async Task<IPagedList<ProductDto>> GetUsingPagedListAsync(int pageNumber, int pageSize,
            Expression<Func<ProductDto, bool>> predicate = null)
        {
            return await ProductUnitOfWork.GetUsingPagedListAsync(pageNumber, pageSize, predicate)
                .ConfigureAwait(false);
        }

        public async Task<ProductDto> GetAsync(int id)
        {
            return await ProductUnitOfWork.GetAsync(id).ConfigureAwait(false);
        }

        public async Task<ProductDto> AddAsync(ProductDto model)
        {
            return await ProductUnitOfWork.AddAsync(model).ConfigureAwait(false);
        }

        public async Task<ProductDto> UpdateAsync(ProductDto model)
        {
            return await ProductUnitOfWork.UpdateAsync(model).ConfigureAwait(false);
        }

        public async Task DeleteAsync(int id)
        {
            await ProductUnitOfWork.DeleteAsync(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<ProductDto>> FindAsync(Expression<Func<ProductDto, bool>> predicate)
        {
            return await ProductUnitOfWork.FindAsync(predicate).ConfigureAwait(false);
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