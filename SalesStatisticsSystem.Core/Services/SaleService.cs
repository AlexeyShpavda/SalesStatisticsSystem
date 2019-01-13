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
    public class SaleService : ISaleService, IDisposable
    {
        private SalesInformationEntities Context { get; }

        private ReaderWriterLockSlim Locker { get; }

        private ISaleUnitOfWork SaleUnitOfWork { get; }

        public SaleService()
        {
            Context = new SalesInformationEntities();

            Locker = new ReaderWriterLockSlim();

            SaleUnitOfWork = new SaleUnitOfWork(Context, Locker);
        }

        public async Task<IPagedList<SaleDto>> GetUsingPagedListAsync(int pageNumber, int pageSize,
            Expression<Func<SaleDto, bool>> predicate = null)
        {
            return await SaleUnitOfWork.GetUsingPagedListAsync(pageNumber, pageSize, predicate)
                .ConfigureAwait(false);
        }

        public async Task<SaleDto> GetAsync(int id)
        {
            return await SaleUnitOfWork.GetAsync(id).ConfigureAwait(false);
        }

        public async Task<SaleDto> AddAsync(SaleDto model)
        {
            return await SaleUnitOfWork.AddAsync(model).ConfigureAwait(false);
        }

        public async Task<SaleDto> UpdateAsync(SaleDto model)
        {
            return await SaleUnitOfWork.UpdateAsync(model).ConfigureAwait(false);
        }

        public async Task DeleteAsync(int id)
        {
            await SaleUnitOfWork.DeleteAsync(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<SaleDto>> FindAsync(Expression<Func<SaleDto, bool>> predicate)
        {
            return await SaleUnitOfWork.FindAsync(predicate).ConfigureAwait(false);
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