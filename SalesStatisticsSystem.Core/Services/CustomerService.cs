using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;
using SalesStatisticsSystem.Contracts.Core.Services;
using SalesStatisticsSystem.Contracts.DataAccessLayer.ReaderWriter;
using SalesStatisticsSystem.DataAccessLayer.ReaderWriter;
using SalesStatisticsSystem.Entity;
using X.PagedList;

namespace SalesStatisticsSystem.Core.Services
{
    public class CustomerService : ICustomerService, IDisposable
    {
        private SalesInformationEntities Context { get; }

        private ReaderWriterLockSlim Locker { get; }

        private ICustomerDbReaderWriter CustomerDbReaderWriter { get; }

        public CustomerService()
        {
            Context = new SalesInformationEntities();

            Locker = new ReaderWriterLockSlim();

            CustomerDbReaderWriter = new CustomerDbReaderWriter(Context, Locker);
        }

        public async Task<IPagedList<CustomerDto>> GetUsingPagedListAsync(int pageNumber, int pageSize,
            Expression<Func<CustomerDto, bool>> predicate = null,
            SortDirection sortDirection = SortDirection.Ascending)
        {
            return await CustomerDbReaderWriter.GetUsingPagedListAsync(pageNumber, pageSize, predicate)
                .ConfigureAwait(false);
        }

        public async Task<CustomerDto> GetAsync(int id)
        {
            return await CustomerDbReaderWriter.GetAsync(id).ConfigureAwait(false);
        }

        public async Task<CustomerDto> AddAsync(CustomerDto model)
        {
            return await CustomerDbReaderWriter.AddAsync(model).ConfigureAwait(false);      
        }

        public async Task<CustomerDto> UpdateAsync(CustomerDto model)
        {
            return await CustomerDbReaderWriter.UpdateAsync(model).ConfigureAwait(false);
        }

        public async Task DeleteAsync(int id)
        {
            await CustomerDbReaderWriter.DeleteAsync(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<CustomerDto>> FindAsync(Expression<Func<CustomerDto, bool>> predicate)
        {
            return await CustomerDbReaderWriter.FindAsync(predicate).ConfigureAwait(false);
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