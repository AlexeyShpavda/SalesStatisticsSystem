using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;
using SalesStatisticsSystem.Contracts.Core.Services;
using SalesStatisticsSystem.Contracts.DataAccessLayer.UnitOfWorks;
using SalesStatisticsSystem.DataAccessLayer.UnitOfWorks;
using SalesStatisticsSystem.Entity;
using X.PagedList;

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

        public async Task<IPagedList<CustomerDto>> GetUsingPagedListAsync(int pageNumber, int pageSize,
            Expression<Func<CustomerDto, bool>> predicate = null)
        {
            return await CustomerUnitOfWork.GetUsingPagedListAsync(pageNumber, pageSize, predicate)
                .ConfigureAwait(false);
        }

        public async Task<CustomerDto> GetAsync(int id)
        {
            return await CustomerUnitOfWork.GetAsync(id).ConfigureAwait(false);
        }

        public async Task<CustomerDto> AddAsync(CustomerDto model)
        {
            return await CustomerUnitOfWork.AddAsync(model).ConfigureAwait(false);      
        }

        public async Task<CustomerDto> UpdateAsync(CustomerDto model)
        {
            return await CustomerUnitOfWork.UpdateAsync(model).ConfigureAwait(false);
        }

        public async Task DeleteAsync(int id)
        {
            await CustomerUnitOfWork.DeleteAsync(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<CustomerDto>> FindAsync(Expression<Func<CustomerDto, bool>> predicate)
        {
            return await CustomerUnitOfWork.FindAsync(predicate).ConfigureAwait(false);
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