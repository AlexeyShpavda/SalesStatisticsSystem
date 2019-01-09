using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;
using SalesStatisticsSystem.Contracts.DataAccessLayer.Repositories;
using SalesStatisticsSystem.Contracts.DataAccessLayer.UnitOfWorks;
using SalesStatisticsSystem.DataAccessLayer.Repositories;
using SalesStatisticsSystem.Entity;

namespace SalesStatisticsSystem.DataAccessLayer.UnitOfWorks
{
    public class ProductUnitOfWork : IProductUnitOfWork
    {
        private SalesInformationEntities Context { get; }

        private ReaderWriterLockSlim Locker { get; }

        private IProductRepository Products { get; }

        public ProductUnitOfWork(SalesInformationEntities context, ReaderWriterLockSlim locker)
        {
            Context = context;

            Locker = locker;

            var mapper = Support.AutoMapper.CreateConfiguration().CreateMapper();

            Products = new ProductRepository(Context, mapper);
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            return await Products.GetAllAsync();
        }

        public ProductDto GetAsync(int id)
        {
            return Products.Get(id);
        }

        public async Task<ProductDto> AddAsync(ProductDto product)
        {
            Locker.EnterWriteLock();
            try
            {
                var result = Products.AddUniqueProductToDatabase(product);
                await Products.SaveAsync();

                return result;
            }
            finally
            {
                Locker.ExitWriteLock();
            }
        }

        public async Task<ProductDto> UpdateAsync(ProductDto product)
        {
            Locker.EnterWriteLock();
            try
            {
                if (Products.DoesProductExist(product)) throw new ArgumentException("Product already exists!");

                var result = Products.Update(product);
                await Products.SaveAsync();

                return result;
            }
            finally
            {
                Locker.ExitWriteLock();
            }
        }

        public void Delete(int id)
        {
            Locker.EnterReadLock();
            try
            {
                Products.Remove(id);
                Products.SaveAsync();
            }
            finally
            {
                Locker.ExitReadLock();
            }
        }
    }
}