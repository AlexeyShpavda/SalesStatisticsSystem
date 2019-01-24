using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;
using SalesStatisticsSystem.Core.Contracts.Models.Sales;
using SalesStatisticsSystem.DataAccessLayer.Contracts.ReaderWriter;
using SalesStatisticsSystem.DataAccessLayer.Contracts.Repository;
using SalesStatisticsSystem.DataAccessLayer.Repositories;
using SalesStatisticsSystem.Entity;
using X.PagedList;

namespace SalesStatisticsSystem.DataAccessLayer.ReaderWriter
{
    public class CustomerDbReaderWriter : ICustomerDbReaderWriter
    {
        private SalesInformationEntities Context { get; }

        private ReaderWriterLockSlim Locker { get; }

        private ICustomerRepository Customers { get; }

        public CustomerDbReaderWriter(SalesInformationEntities context, ReaderWriterLockSlim locker)
        {
            Context = context;

            Locker = locker;

            var mapper = Support.Adapter.AutoMapper.CreateConfiguration().CreateMapper();

            Customers = new CustomerRepository(Context, mapper);
        }

        public async Task<IPagedList<CustomerCoreModel>> GetUsingPagedListAsync(int number, int size,
            Expression<Func<CustomerCoreModel, bool>> predicate = null,
            SortDirection sortDirection = SortDirection.Ascending)
        {
            return await Customers.GetUsingPagedListAsync(number, size, predicate).ConfigureAwait(false);
        }

        public async Task<CustomerCoreModel> GetAsync(int id)
        {
            return await Customers.GetAsync(id).ConfigureAwait(false);
        }

        public async Task<CustomerCoreModel> AddAsync(CustomerCoreModel customer)
        {
            Locker.EnterWriteLock();
            try
            {
                if (await Customers.TryAddUniqueCustomerAsync(customer).ConfigureAwait(false))
                {
                    await Customers.SaveAsync().ConfigureAwait(false);
                    return await GetAsync(await Customers.GetIdAsync(customer.FirstName, customer.LastName)
                        .ConfigureAwait(false)).ConfigureAwait(false);
                }
                else
                {
                    throw new ArgumentException("Customer already exists!");
                }
            }
            finally
            {
                if (Locker.IsWriteLockHeld)
                {
                    Locker.ExitWriteLock();
                }
            }
        }

        public async Task<CustomerCoreModel> UpdateAsync(CustomerCoreModel customer)
        {
            Locker.EnterWriteLock();
            try
            {
                if (await Customers.DoesCustomerExistAsync(customer).ConfigureAwait(false))
                    throw new ArgumentException("Customer already exists!");

                var result = Customers.Update(customer);
                await Customers.SaveAsync().ConfigureAwait(false);

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
                await Customers.DeleteAsync(id).ConfigureAwait(false);
                await Customers.SaveAsync().ConfigureAwait(false);
            }
            finally
            {
                if (Locker.IsReadLockHeld)
                {
                    Locker.ExitReadLock();
                }
            }
        }

        public async Task<IEnumerable<CustomerCoreModel>> FindAsync(Expression<Func<CustomerCoreModel, bool>> predicate)
        {
            return await Customers.FindAsync(predicate).ConfigureAwait(false);
        }
    }
}