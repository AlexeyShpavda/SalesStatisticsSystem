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
            return await Customers.GetAllAsync();
        }

        public async Task<CustomerDto> GetAsync(int id)
        {
            return await Customers.GetAsync(id);
        }

        public async Task<CustomerDto> AddAsync(CustomerDto customer)
        {
            Locker.EnterWriteLock();
            try
            {
                var result = await Customers.AddUniqueCustomerToDatabaseAsync(customer);

                await Customers.SaveAsync();

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
                if (await Customers.DoesCustomerExistAsync(customer)) throw new ArgumentException("Customer already exists!");

                var result = Customers.Update(customer);
                await Customers.SaveAsync();

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
                await Customers.DeleteAsync(id);
                await Customers.SaveAsync();
            }
            finally
            {
                if (Locker.IsReadLockHeld)
                {
                    Locker.ExitReadLock();
                }
            }
        }
    }
}