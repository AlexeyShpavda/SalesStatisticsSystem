﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;
using SalesStatisticsSystem.Contracts.DataAccessLayer.Repositories;
using SalesStatisticsSystem.Contracts.DataAccessLayer.UnitOfWorks;
using SalesStatisticsSystem.DataAccessLayer.Repositories;
using SalesStatisticsSystem.Entity;
using X.PagedList;

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

        public async Task<IPagedList<SaleDto>> GetUsingPagedListAsync(int number, int size,
            Expression<Func<SaleDto, bool>> predicate = null, SortDirection sortDirection = SortDirection.Ascending)
        {
            return await Sales.GetUsingPagedListAsync(number, size, predicate).ConfigureAwait(false);
        }

        public async Task<SaleDto> GetAsync(int id)
        {
            return await Sales.GetAsync(id).ConfigureAwait(false);
        }

        public async Task<SaleDto> AddAsync(SaleDto sale)
        {
            Locker.EnterWriteLock();
            try
            {
                await FindOutIds(sale).ConfigureAwait(false);

                var result = Sales.Add(sale);
                await Sales.SaveAsync().ConfigureAwait(false);

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
                await FindOutIds(sale).ConfigureAwait(false);

                var result = Sales.Update(sale);
                await Sales.SaveAsync().ConfigureAwait(false);

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
                await Sales.DeleteAsync(id).ConfigureAwait(false);
                await Sales.SaveAsync().ConfigureAwait(false);
            }
            finally
            {
                if (Locker.IsReadLockHeld)
                {
                    Locker.ExitReadLock();
                }
            }
        }

        public async Task<IEnumerable<SaleDto>> FindAsync(Expression<Func<SaleDto, bool>> predicate)
        {
            return await Sales.FindAsync(predicate).ConfigureAwait(false);
        }

        private async Task FindOutIds(SaleDto sale)
        {
            if (await Customers.DoesCustomerExistAsync(sale.Customer).ConfigureAwait(false))
            {
                sale.Customer.Id = await Customers.GetIdAsync(sale.Customer.FirstName, sale.Customer.LastName)
                    .ConfigureAwait(false);
            }
            else
            {
                ThrowArgumentException("There is no such Customer. Register it!");
            }

            if (await Managers.DoesManagerExistAsync(sale.Manager))
            {
                sale.Manager.Id = await Managers.GetIdAsync(sale.Manager.LastName).ConfigureAwait(false);
            }
            else
            {
                ThrowArgumentException("There is no such Manager. Register it!");
            }

            if (await Products.DoesProductExistAsync(sale.Product))
            {
                sale.Product.Id = await Products.GetIdAsync(sale.Product.Name).ConfigureAwait(false);
            }
            else
            {
                ThrowArgumentException("There is no such Product. Register it!");
            }
        }

        private static void ThrowArgumentException(string message)
        {
            throw new ArgumentException(message);
        }
    }
}