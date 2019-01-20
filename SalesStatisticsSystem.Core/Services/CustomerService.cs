using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;
using SalesStatisticsSystem.Core.Contracts.Models;
using SalesStatisticsSystem.Core.Contracts.Services;
using SalesStatisticsSystem.DataAccessLayer.Contracts.ReaderWriter;
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

        public async Task<IPagedList<CustomerCoreModel>> GetUsingPagedListAsync(int pageNumber, int pageSize,
            Expression<Func<CustomerCoreModel, bool>> predicate = null,
            SortDirection sortDirection = SortDirection.Ascending)
        {
            return await CustomerDbReaderWriter.GetUsingPagedListAsync(pageNumber, pageSize, predicate)
                .ConfigureAwait(false);
        }

        public async Task<CustomerCoreModel> GetAsync(int id)
        {
            return await CustomerDbReaderWriter.GetAsync(id).ConfigureAwait(false);
        }

        public async Task<CustomerCoreModel> AddAsync(CustomerCoreModel model)
        {
            return await CustomerDbReaderWriter.AddAsync(model).ConfigureAwait(false);      
        }

        public async Task<CustomerCoreModel> UpdateAsync(CustomerCoreModel model)
        {
            return await CustomerDbReaderWriter.UpdateAsync(model).ConfigureAwait(false);
        }

        public async Task DeleteAsync(int id)
        {
            await CustomerDbReaderWriter.DeleteAsync(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<CustomerCoreModel>> FindAsync(Expression<Func<CustomerCoreModel, bool>> predicate)
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