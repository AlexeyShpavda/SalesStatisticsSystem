using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;
using SalesStatisticsSystem.Contracts.DataAccessLayer.Repositories;
using SalesStatisticsSystem.Contracts.DataAccessLayer.UnitOfWorks;
using SalesStatisticsSystem.DataAccessLayer.Repositories;
using SalesStatisticsSystem.Entity;

namespace SalesStatisticsSystem.DataAccessLayer.UnitOfWorks
{
    public class ManagerUnitOfWork : IManagerUnitOfWork
    {
        private SalesInformationEntities Context { get; }

        private ReaderWriterLockSlim Locker { get; }

        private IManagerRepository Managers { get; }

        public ManagerUnitOfWork(SalesInformationEntities context, ReaderWriterLockSlim locker)
        {
            Context = context;

            Locker = locker;

            var mapper = Support.AutoMapper.CreateConfiguration().CreateMapper();

            Managers = new ManagerRepository(Context, mapper);
        }

        public async Task<IEnumerable<ManagerDto>> GetAllAsync()
        {
            return await Managers.GetAllAsync();
        }

        public ManagerDto GetAsync(int id)
        {
            return Managers.Get(id);
        }

        public void Add(params ManagerDto[] products)
        {
            Locker.EnterWriteLock();
            try
            {
                foreach (var product in products)
                {
                    Managers.AddUniqueManagerToDatabase(product);
                    Managers.Save();
                }
            }
            finally
            {
                Locker.ExitWriteLock();
            }
        }

        public void Update(params ManagerDto[] products)
        {
            Locker.EnterWriteLock();
            try
            {
                foreach (var product in products)
                {
                    Managers.Update(product);
                    Managers.Save();
                }
            }
            finally
            {
                Locker.ExitWriteLock();
            }
        }

        public void Delete(params ManagerDto[] products)
        {
            Locker.EnterReadLock();
            try
            {
                foreach (var product in products)
                {
                    Managers.Remove(product);
                    Managers.Save();
                }
            }
            finally
            {
                Locker.ExitReadLock();
            }
        }

        public void Delete(int id)
        {
            Locker.EnterReadLock();
            try
            {
                Managers.Remove(id);
                Managers.Save();
            }
            finally
            {
                Locker.ExitReadLock();
            }
        }
    }
}