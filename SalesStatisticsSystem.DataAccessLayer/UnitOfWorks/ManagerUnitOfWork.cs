using System;
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

            var mapper = Support.Adapter.AutoMapper.CreateConfiguration().CreateMapper();

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

        public async Task<ManagerDto> AddAsync(ManagerDto manager)
        {
            Locker.EnterWriteLock();
            try
            {
                var result = Managers.AddUniqueManagerToDatabase(manager);
                await Managers.SaveAsync();

                return result;
            }
            finally
            {
                if (Locker.IsWriteLockHeld)
                {
                    Locker.ExitWriteLock();
                }
            }
        }

        public async Task<ManagerDto> UpdateAsync(ManagerDto manager)
        {
            Locker.EnterWriteLock();
            try
            {
                if (Managers.DoesManagerExist(manager)) throw new ArgumentException("Manager already exists!");

                var result = Managers.Update(manager);
                await Managers.SaveAsync();

                return result;
            }
            finally
            {
                if (Locker.IsWriteLockHeld)
                {
                    Locker.ExitWriteLock();
                }
            }
        }

        public void Delete(int id)
        {
            Locker.EnterReadLock();
            try
            {
                Managers.Remove(id);
                Managers.SaveAsync();
            }
            finally
            {
                if (Locker.IsReadLockHeld)
                {
                    Locker.ExitReadLock();
                }
            }
        }
    }
}