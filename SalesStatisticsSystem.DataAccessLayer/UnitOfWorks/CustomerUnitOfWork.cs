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

            var mapper = Support.AutoMapper.CreateConfiguration().CreateMapper();

            Customers = new CustomerRepository(Context, mapper);
        }

        public async Task<IEnumerable<CustomerDto>> GetAllAsync()
        {
            return await Customers.GetAllAsync();
        }

        public CustomerDto GetAsync(int id)
        {
            return Customers.Get(id);
        }

        public async Task<CustomerDto> AddAsync(CustomerDto customer)
        {
            Locker.EnterWriteLock();
            try
            {
                var result = Customers.AddUniqueCustomerToDatabase(customer);

                await Customers.SaveAsync();

                return result;
            }
            finally
            {
                Locker.ExitWriteLock();
            }
        }

        public async Task<CustomerDto> UpdateAsync(CustomerDto customer)
        {
            Locker.EnterWriteLock();
            try
            {
                if (Customers.DoesCustomerExist(customer)) throw new ArgumentException("Customer already exists!");

                var result = Customers.Update(customer);
                await Customers.SaveAsync();

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
                Customers.Remove(id);
                Customers.SaveAsync();
            }
            finally
            {
                Locker.ExitReadLock();
            }
        }
    }
}