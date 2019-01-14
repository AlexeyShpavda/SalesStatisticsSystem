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

        public async Task<IPagedList<ManagerDto>> GetUsingPagedListAsync(int pageNumber, int pageSize,
            Expression<Func<ManagerDto, bool>> predicate = null, SortDirection sortDirection = SortDirection.Ascending)
        {
            return await ManagerDbReaderWriter.GetUsingPagedListAsync(pageNumber, pageSize, predicate)
                .ConfigureAwait(false);
        }

        public async Task<ManagerDto> GetAsync(int id)
        {
            return await ManagerDbReaderWriter.GetAsync(id).ConfigureAwait(false);
        }

        public async Task<ManagerDto> AddAsync(ManagerDto model)
        {
            return await ManagerDbReaderWriter.AddAsync(model).ConfigureAwait(false);
        }

        public async Task<ManagerDto> UpdateAsync(ManagerDto model)
        {
            return await ManagerDbReaderWriter.UpdateAsync(model).ConfigureAwait(false);
        }

        public async Task DeleteAsync(int id)
        {
            await ManagerDbReaderWriter.DeleteAsync(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<ManagerDto>> FindAsync(Expression<Func<ManagerDto, bool>> predicate)
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