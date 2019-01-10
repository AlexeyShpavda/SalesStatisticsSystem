using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;
using SalesStatisticsSystem.Contracts.Core.Services;
using SalesStatisticsSystem.Contracts.DataAccessLayer.UnitOfWorks;
using SalesStatisticsSystem.DataAccessLayer.UnitOfWorks;
using SalesStatisticsSystem.Entity;

namespace SalesStatisticsSystem.Core.Services
{
    public class CustomerService : ICustomerService, IDisposable
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

        public async Task<CustomerDto> GetAsync(int id)
        {
            return await CustomerUnitOfWork.GetAsync(id);
        }

        public async Task<CustomerDto> AddAsync(CustomerDto model)
        {
            return await CustomerUnitOfWork.AddAsync(model);      
        }

        public async Task<CustomerDto> UpdateAsync(CustomerDto model)
        {
            return await CustomerUnitOfWork.UpdateAsync(model);
        }

        public async Task DeleteAsync(int id)
        {
            await CustomerUnitOfWork.DeleteAsync(id);
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