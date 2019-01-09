using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;
using SalesStatisticsSystem.Contracts.Core.Services;
using SalesStatisticsSystem.Contracts.DataAccessLayer.UnitOfWorks;
using SalesStatisticsSystem.DataAccessLayer.UnitOfWorks;
using SalesStatisticsSystem.Entity;

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

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            return await ProductUnitOfWork.GetAllAsync();
        }

        // TODO: Make async
        public ProductDto GetAsync(int id)
        {
            return ProductUnitOfWork.GetAsync(id);
        }

        public async Task<ProductDto> AddAsync(ProductDto model)
        {
            return await ProductUnitOfWork.AddAsync(model);
        }

        public async Task<ProductDto> UpdateAsync(ProductDto model)
        {
            return await ProductUnitOfWork.UpdateAsync(model);
        }

        public void Delete(int id)
        {
            ProductUnitOfWork.Delete(id);
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