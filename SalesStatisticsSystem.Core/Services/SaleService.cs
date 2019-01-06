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

        public async Task<IEnumerable<SaleDto>> GetAllAsync()
        {
            return await SaleUnitOfWork.GetAllAsync();
        }

        // TODO: Make async
        public SaleDto GetAsync(int id)
        {
            return SaleUnitOfWork.GetAsync(id);
        }

        public void Add(params SaleDto[] models)
        {
            SaleUnitOfWork.Add(models);
        }

        public void Update(params SaleDto[] models)
        {
            SaleUnitOfWork.Update(models);
        }

        public void Delete(params SaleDto[] models)
        {
            SaleUnitOfWork.Delete(models);
        }

        public void Delete(int id)
        {
            SaleUnitOfWork.Delete(id);
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