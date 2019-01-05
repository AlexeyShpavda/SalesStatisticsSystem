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

        public async Task<IEnumerable<ProductDto>> GetAsync()
        {
            return await Products.GetAllAsync();
        }
    }
}