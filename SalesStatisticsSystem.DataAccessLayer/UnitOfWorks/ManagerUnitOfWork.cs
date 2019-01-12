using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
            return await Managers.GetAllAsync().ConfigureAwait(false);
        }

        public async Task<ManagerDto> GetAsync(int id)
        {
            return await Managers.GetAsync(id).ConfigureAwait(false);
        }

        public async Task<ManagerDto> AddAsync(ManagerDto manager)
        {
            Locker.EnterWriteLock();
            try
            {
                if (await Managers.TryAddUniqueManagerAsync(manager).ConfigureAwait(false))
                {
                    await Managers.SaveAsync().ConfigureAwait(false);
                    return await GetAsync(await Managers.GetIdAsync(manager.LastName)
                        .ConfigureAwait(false)).ConfigureAwait(false);
                }
                else
                {
                    throw new ArgumentException("Manager already exists!");
                }
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
                if (await Managers.DoesManagerExistAsync(manager).ConfigureAwait(false))
                    throw new ArgumentException("Manager already exists!");

                var result = Managers.Update(manager);
                await Managers.SaveAsync().ConfigureAwait(false);

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

        public async Task DeleteAsync(int id)
        {
            Locker.EnterReadLock();
            try
            {
                await Managers.DeleteAsync(id).ConfigureAwait(false);
                await Managers.SaveAsync().ConfigureAwait(false);
            }
            finally
            {
                if (Locker.IsReadLockHeld)
                {
                    Locker.ExitReadLock();
                }
            }
        }

        public async Task<IEnumerable<ManagerDto>> FindAsync(Expression<Func<ManagerDto, bool>> predicate)
        {
            return await Managers.FindAsync(predicate).ConfigureAwait(false);
        }
    }
}