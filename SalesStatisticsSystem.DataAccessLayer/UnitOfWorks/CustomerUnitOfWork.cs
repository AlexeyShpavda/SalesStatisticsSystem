using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;
using SalesStatisticsSystem.Contracts.DataAccessLayer.Repositories;
using SalesStatisticsSystem.Contracts.DataAccessLayer.UnitOfWorks;
using SalesStatisticsSystem.DataAccessLayer.Repositories;
using SalesStatisticsSystem.Entity;

namespace SalesStatisticsSystem.DataAccessLayer.UnitOfWorks
{
    public class CustomerUnitOfWork : ICustomerUnitOfWork
    {
        private SalesInformationEntities Context { get; }

        private ReaderWriterLockSlim Locker { get; }

        private ICustomerRepository Customers { get; }

        public CustomerUnitOfWork(SalesInformationEntities context, ReaderWriterLockSlim locker)
        {
            Context = context;

            Locker = locker;

            var mapper = Support.Adapter.AutoMapper.CreateConfiguration().CreateMapper();

            Customers = new CustomerRepository(Context, mapper);
        }

        public async Task<IEnumerable<CustomerDto>> GetAllAsync()
        {
            return await Customers.GetAllAsync().ConfigureAwait(false);
        }

        public async Task<CustomerDto> GetAsync(int id)
        {
            return await Customers.GetAsync(id).ConfigureAwait(false);
        }

        public async Task<CustomerDto> AddAsync(CustomerDto customer)
        {
            Locker.EnterWriteLock();
            try
            {
                var result = await Customers.AddUniqueCustomerToDatabaseAsync(customer).ConfigureAwait(false);

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

        public async Task<CustomerDto> UpdateAsync(CustomerDto customer)
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

        public async Task<IEnumerable<CustomerDto>> FindAsync(Expression<Func<CustomerDto, bool>> predicate)
        {
            return await Customers.FindAsync(predicate).ConfigureAwait(false);
        }
    }
}