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
    public class ManagerService : IManagerService, IDisposable
    {
        private SalesInformationEntities Context { get; }

        private ReaderWriterLockSlim Locker { get; }

        private IManagerUnitOfWork ManagerUnitOfWork { get; }

        public ManagerService()
        {
            Context = new SalesInformationEntities();

            Locker = new ReaderWriterLockSlim();

            ManagerUnitOfWork = new ManagerUnitOfWork(Context, Locker);
        }

        public async Task<IPagedList<ManagerDto>> GetUsingPagedListAsync(int pageNumber, int pageSize,
            Expression<Func<ManagerDto, bool>> predicate = null)
        {
            return await ManagerUnitOfWork.GetUsingPagedListAsync(pageNumber, pageSize, predicate)
                .ConfigureAwait(false);
        }

        public async Task<ManagerDto> GetAsync(int id)
        {
            return await ManagerUnitOfWork.GetAsync(id).ConfigureAwait(false);
        }

        public async Task<ManagerDto> AddAsync(ManagerDto model)
        {
            return await ManagerUnitOfWork.AddAsync(model).ConfigureAwait(false);
        }

        public async Task<ManagerDto> UpdateAsync(ManagerDto model)
        {
            return await ManagerUnitOfWork.UpdateAsync(model).ConfigureAwait(false);
        }

        public async Task DeleteAsync(int id)
        {
            await ManagerUnitOfWork.DeleteAsync(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<ManagerDto>> FindAsync(Expression<Func<ManagerDto, bool>> predicate)
        {
            return await ManagerUnitOfWork.FindAsync(predicate).ConfigureAwait(false);
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