using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;
using SalesStatisticsSystem.Core.Contracts.Models.Sales;
using SalesStatisticsSystem.Core.Contracts.Services;
using SalesStatisticsSystem.DataAccessLayer.Contracts.ReaderWriter;
using SalesStatisticsSystem.DataAccessLayer.ReaderWriter;
using SalesStatisticsSystem.Entity;
using X.PagedList;

namespace SalesStatisticsSystem.Core.Services
{
    public class SaleService : ISaleService, IDisposable
    {
        private SalesInformationEntities Context { get; }

        private ReaderWriterLockSlim Locker { get; }

        private ISaleDbReaderWriter SaleDbReaderWriter { get; }

        public SaleService()
        {
            Context = new SalesInformationEntities();

            Locker = new ReaderWriterLockSlim();

            SaleDbReaderWriter = new SaleDbReaderWriter(Context, Locker);
        }

        public async Task<IPagedList<SaleCoreModel>> GetUsingPagedListAsync(int pageNumber, int pageSize,
            Expression<Func<SaleCoreModel, bool>> predicate = null, SortDirection sortDirection = SortDirection.Ascending)
        {
            return await SaleDbReaderWriter.GetUsingPagedListAsync(pageNumber, pageSize, predicate)
                .ConfigureAwait(false);
        }

        public async Task<SaleCoreModel> GetAsync(int id)
        {
            return await SaleDbReaderWriter.GetAsync(id).ConfigureAwait(false);
        }

        public async Task<SaleCoreModel> AddAsync(SaleCoreModel model)
        {
            return await SaleDbReaderWriter.AddAsync(model).ConfigureAwait(false);
        }

        public async Task<SaleCoreModel> UpdateAsync(SaleCoreModel model)
        {
            return await SaleDbReaderWriter.UpdateAsync(model).ConfigureAwait(false);
        }

        public async Task DeleteAsync(int id)
        {
            await SaleDbReaderWriter.DeleteAsync(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<SaleCoreModel>> FindAsync(Expression<Func<SaleCoreModel, bool>> predicate)
        {
            return await SaleDbReaderWriter.FindAsync(predicate).ConfigureAwait(false);
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