using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;
using SalesStatisticsSystem.Contracts.DataAccessLayer.UnitOfWorks;
using SalesStatisticsSystem.DataAccessLayer.UnitOfWorks;
using SalesStatisticsSystem.Entity;

namespace SalesStatisticsSystem.Core.Services
{
    public class SaleService : IDisposable
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


        public async Task<IEnumerable<SaleDto>> GetSalesAsync()
        {
            return await SaleUnitOfWork.GetAllAsync();
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