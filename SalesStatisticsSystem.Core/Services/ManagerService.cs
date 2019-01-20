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
    public class ManagerService : IManagerService, IDisposable
    {
        private SalesInformationEntities Context { get; }

        private ReaderWriterLockSlim Locker { get; }

        private IManagerDbReaderWriter ManagerDbReaderWriter { get; }

        public ManagerService()
        {
            Context = new SalesInformationEntities();

            Locker = new ReaderWriterLockSlim();

            ManagerDbReaderWriter = new ManagerDbReaderWriter(Context, Locker);
        }

        public async Task<IPagedList<ManagerCoreModel>> GetUsingPagedListAsync(int pageNumber, int pageSize,
            Expression<Func<ManagerCoreModel, bool>> predicate = null, SortDirection sortDirection = SortDirection.Ascending)
        {
            return await ManagerDbReaderWriter.GetUsingPagedListAsync(pageNumber, pageSize, predicate)
                .ConfigureAwait(false);
        }

        public async Task<ManagerCoreModel> GetAsync(int id)
        {
            return await ManagerDbReaderWriter.GetAsync(id).ConfigureAwait(false);
        }

        public async Task<ManagerCoreModel> AddAsync(ManagerCoreModel model)
        {
            return await ManagerDbReaderWriter.AddAsync(model).ConfigureAwait(false);
        }

        public async Task<ManagerCoreModel> UpdateAsync(ManagerCoreModel model)
        {
            return await ManagerDbReaderWriter.UpdateAsync(model).ConfigureAwait(false);
        }

        public async Task DeleteAsync(int id)
        {
            await ManagerDbReaderWriter.DeleteAsync(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<ManagerCoreModel>> FindAsync(Expression<Func<ManagerCoreModel, bool>> predicate)
        {
            return await ManagerDbReaderWriter.FindAsync(predicate).ConfigureAwait(false);
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