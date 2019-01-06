using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;
using SalesStatisticsSystem.Entity;

namespace SalesStatisticsSystem.Core.Services
{
    public class CustomerService : IDisposable
    {
        private SalesInformationEntities Context { get; }

        private ReaderWriterLockSlim Locker { get; }

        private ICustomerUnitOfWork CustomerUnitOfWork { get; }

        public CustomerService()
        {
            Context = new SalesInformationEntities();

            Locker = new ReaderWriterLockSlim();

            CustomerUnitOfWork = new CustomerUnitOfWork(Context, Locker);
        }


        public async Task<IEnumerable<CustomerDto>> GetAllAsync()
        {
            return await CustomerUnitOfWork.GetAllAsync();
        }


        // TODO: Make async
        public CustomerDto GetAsync(int id)
        {
            return CustomertUnitOfWork.GetAsync(id);
        }

        public void Add(params CustomerDto[] models)
        {
            CustomerUnitOfWork.Add(models);
        }

        public void Update(params CustomerDto[] models)
        {
            CustomerUnitOfWork.Update(models);
        }

        public void Delete(params CustomerDto[] models)
        {
            CustomerUnitOfWork.Delete(models);
        }

        public void Delete(int id)
        {
            CustomerUnitOfWork.Delete(id);
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Locker.Dispose();
                    Context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}