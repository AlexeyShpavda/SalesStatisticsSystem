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
    public class SaleUnitOfWork : ISaleUnitOfWork
    {
        private SalesInformationEntities Context { get; }
        private ReaderWriterLockSlim Locker { get; }

        private ICustomerRepository Customers { get; }
        private IManagerRepository Managers { get; }
        private IProductRepository Products { get; }
        private ISaleRepository Sales { get; }

        public SaleUnitOfWork(SalesInformationEntities context, ReaderWriterLockSlim locker)
        {
            Context = context;
            Locker = locker;

            var mapper = Support.Adapter.AutoMapper.CreateConfiguration().CreateMapper();
            Customers = new CustomerRepository(Context, mapper);
            Managers = new ManagerRepository(Context, mapper);
            Products = new ProductRepository(Context, mapper);
            Sales = new SaleRepository(Context, mapper);
        }

        public async Task<IEnumerable<SaleDto>> GetAllAsync()
        {
            return await Sales.GetAllAsync();
        }

        public async Task<SaleDto> GetAsync(int id)
        {
            return await Sales.GetAsync(id);
        }

        public async Task<SaleDto> AddAsync(SaleDto sale)
        {
            Locker.EnterWriteLock();
            try
            {
                await FindOutIds(sale);

                var result = Sales.Add(sale);
                await Sales.SaveAsync();

                return result;
            }
            finally
            {
                if (Locker.IsWriteLockHeld)
                {
                    Locker.ExitWriteLock();
                }
            }
        }

        public async Task<SaleDto> UpdateAsync(SaleDto sale)
        {
            Locker.EnterWriteLock();
            try
            {
                await FindOutIds(sale);

                var result = Sales.Update(sale);
                await Sales.SaveAsync();

                return result;
            }
            finally
            {
                if (Locker.IsWriteLockHeld)
                {
                    Locker.ExitWriteLock();
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
            Locker.EnterReadLock();
            try
            {
                await Sales.DeleteAsync(id);
                await Sales.SaveAsync();
            }
            finally
            {
                if (Locker.IsReadLockHeld)
                {
                    Locker.ExitReadLock();
                }
            }
        }

        private async Task FindOutIds(SaleDto sale)
        {
            if (await Customers.DoesCustomerExistAsync(sale.Customer))
            {
                sale.Customer.Id = await Customers.GetIdAsync(sale.Customer.FirstName, sale.Customer.LastName);
            }
            else
            {
                throw new ArgumentException("There is no such Customer. Register it!");
            }

            if (await Managers.DoesManagerExistAsync(sale.Manager))
            {
                sale.Manager.Id = await Managers.GetIdAsync(sale.Manager.LastName);
            }
            else
            {
                throw new ArgumentException("There is no such Manager. Register it!");
            }

            if (await Products.DoesProductExistAsync(sale.Product))
            {
                sale.Product.Id = await Products.GetIdAsync(sale.Product.Name);
            }
            else
            {
                throw new ArgumentException("There is no such Product. Register it!");
            }
        }
    }
}