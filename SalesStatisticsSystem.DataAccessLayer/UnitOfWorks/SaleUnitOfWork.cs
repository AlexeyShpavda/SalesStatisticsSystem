using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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

            var mapper = Support.AutoMapper.CreateConfiguration().CreateMapper();
            Customers = new CustomerRepository(Context, mapper);
            Managers = new ManagerRepository(Context, mapper);
            Products = new ProductRepository(Context, mapper);
            Sales = new SaleRepository(Context, mapper);
        }

        public async Task<IEnumerable<SaleDto>> GetAllAsync()
        {
            return await Sales.GetAllAsync();
        }

        public SaleDto GetAsync(int id)
        {
            return Sales.Get(id);
        }

        public async Task<SaleDto> AddAsync(SaleDto sale)
        {
            Locker.EnterWriteLock();
            try
            {
                FindOutIds(sale);

                var result = Sales.Add(sale);
                await Sales.SaveAsync();

                return result;
            }
            finally
            {
                Locker.ExitWriteLock();
            }
        }

        public async Task<SaleDto> UpdateAsync(SaleDto sale)
        {
            Locker.EnterWriteLock();
            try
            {
                FindOutIds(sale);

                var result = Sales.Update(sale);
                await Sales.SaveAsync();

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
                Sales.Remove(id);
                Sales.SaveAsync();
            }
            finally
            {
                Locker.ExitReadLock();
            }
        }

        private void FindOutIds(SaleDto sale)
        {
            if (Customers.DoesCustomerExist(sale.Customer))
            {
                sale.Customer.Id = Customers.GetId(sale.Customer.FirstName, sale.Customer.LastName);
            }
            else
            {
                throw new ArgumentException("There is no such Customer. Register it!");
            }

            if (Managers.DoesManagerExist(sale.Manager))
            {
                sale.Manager.Id = Managers.GetId(sale.Manager.LastName);
            }
            else
            {
                throw new ArgumentException("There is no such Manager. Register it!");
            }

            if (Products.DoesProductExist(sale.Product))
            {
                sale.Product.Id = Products.GetId(sale.Product.Name);
            }
            else
            {
                throw new ArgumentException("There is no such Product. Register it!");
            }
        }
    }
}