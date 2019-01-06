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

        public void Add(params CustomerDto[] products)
        {
            Locker.EnterWriteLock();
            try
            {
                foreach (var product in products)
                {
                    Customers.AddUniqueCustomerToDatabase(product);
                    Customers.Save();
                }
            }
            finally
            {
                Locker.ExitWriteLock();
            }
        }

        public void Update(params CustomerDto[] products)
        {
            Locker.EnterWriteLock();
            try
            {
                foreach (var product in products)
                {
                    Customers.Update(product);
                    Customers.Save();
                }
            }
            finally
            {
                Locker.ExitWriteLock();
            }
        }

        public void Delete(params CustomerDto[] products)
        {
            Locker.EnterReadLock();
            try
            {
                foreach (var product in products)
                {
                    Customers.Remove(product);
                    Customers.Save();
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
                Customers.Remove(id);
                Customers.Save();
            }
            finally
            {
                Locker.ExitReadLock();
            }
        }
    }
}