using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;
using SalesStatisticsSystem.Core.Contracts.Models;
using SalesStatisticsSystem.DataAccessLayer.Contracts.ReaderWriter;
using SalesStatisticsSystem.DataAccessLayer.Contracts.Repository;
using SalesStatisticsSystem.DataAccessLayer.Repositories;
using SalesStatisticsSystem.Entity;
using X.PagedList;

namespace SalesStatisticsSystem.DataAccessLayer.ReaderWriter
{
    public class ManagerDbReaderWriter : IManagerDbReaderWriter
    {
        private SalesInformationEntities Context { get; }

        private ReaderWriterLockSlim Locker { get; }

        private IManagerRepository Managers { get; }

        public ManagerDbReaderWriter(SalesInformationEntities context, ReaderWriterLockSlim locker)
        {
            Context = context;

            Locker = locker;

            var mapper = Support.Adapter.AutoMapper.CreateConfiguration().CreateMapper();

            Managers = new ManagerRepository(Context, mapper);
        }

        public async Task<IPagedList<ManagerCoreModel>> GetUsingPagedListAsync(int number, int size,
            Expression<Func<ManagerCoreModel, bool>> predicate = null, SortDirection sortDirection = SortDirection.Ascending)
        {
            return await Managers.GetUsingPagedListAsync(number, size, predicate).ConfigureAwait(false);
        }

        public async Task<ManagerCoreModel> GetAsync(int id)
        {
            return await Managers.GetAsync(id).ConfigureAwait(false);
        }

        public async Task<ManagerCoreModel> AddAsync(ManagerCoreModel manager)
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

        public async Task<ManagerCoreModel> UpdateAsync(ManagerCoreModel manager)
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

        public async Task<IEnumerable<ManagerCoreModel>> FindAsync(Expression<Func<ManagerCoreModel, bool>> predicate)
        {
            return await Managers.FindAsync(predicate).ConfigureAwait(false);
        }
    }
}