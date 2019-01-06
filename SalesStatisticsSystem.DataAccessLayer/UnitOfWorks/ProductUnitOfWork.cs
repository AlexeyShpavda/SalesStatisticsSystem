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

        public void Add(params ProductDto[] products)
        {
            Locker.EnterWriteLock();
            try
            {
                foreach (var product in products)
                {
                    Products.AddUniqueProductToDatabase(product);
                    Products.Save();
                }
            }
            finally
            {
                Locker.ExitWriteLock();
            }
        }

        public void Update(params ProductDto[] products)
        {
            Locker.EnterWriteLock();
            try
            {
                foreach (var product in products)
                {
                    Products.Update(product);
                    Products.Save();
                }
            }
            finally
            {
                Locker.ExitWriteLock();
            }
        }

        public void Delete(params ProductDto[] products)
        {
            Locker.EnterReadLock();
            try
            {
                foreach (var product in products)
                {
                    Products.Remove(product);
                    Products.Save();
                }
            }
            finally
            {
                Locker.ExitReadLock();
            }
        }

        public void Delete(int id)
        {
            Locker.EnterReadLock();
            try
            {
                Products.Remove(id);
                Products.Save();
            }
            finally
            {
                Locker.ExitReadLock();
            }
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            return await Products.GetAllAsync();
        }

        public ProductDto GetAsync(int id)
        {
            return Products.Get(id);
        }
    }
}