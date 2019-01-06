using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;
using SalesStatisticsSystem.Entity;

namespace SalesStatisticsSystem.Core.Services
{
    public class ManagerService : IDisposable
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


        public async Task<IEnumerable<ManagerDto>> GetAllAsync()
        {
            return await ManagerUnitOfWork.GetAllAsync();
        }


        // TODO: Make async
        public ManagerDto GetAsync(int id)
        {
            return ManagerUnitOfWork.GetAsync(id);
        }

        public void Add(params ManagerDto[] models)
        {
            ManagerUnitOfWork.Add(models);
        }

        public void Update(params ManagerDto[] models)
        {
            ManagerUnitOfWork.Update(models);
        }

        public void Delete(params ManagerDto[] models)
        {
            ManagerUnitOfWork.Delete(models);
        }

        public void Delete(int id)
        {
            ManagerUnitOfWork.Delete(id);
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