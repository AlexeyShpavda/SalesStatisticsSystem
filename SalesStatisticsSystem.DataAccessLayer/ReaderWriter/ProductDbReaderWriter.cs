﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;
using SalesStatisticsSystem.Contracts.DataAccessLayer.ReaderWriter;
using SalesStatisticsSystem.Contracts.DataAccessLayer.Repositories;
using SalesStatisticsSystem.DataAccessLayer.Repositories;
using SalesStatisticsSystem.Entity;
using X.PagedList;

namespace SalesStatisticsSystem.DataAccessLayer.ReaderWriter
{
    public class ProductDbReaderWriter : IProductDbReaderWriter
    {
        private SalesInformationEntities Context { get; }

        private ReaderWriterLockSlim Locker { get; }

        private IProductRepository Products { get; }

        public ProductDbReaderWriter(SalesInformationEntities context, ReaderWriterLockSlim locker)
        {
            Context = context;

            Locker = locker;

            var mapper = Support.Adapter.AutoMapper.CreateConfiguration().CreateMapper();

            Products = new ProductRepository(Context, mapper);
        }

        public async Task<IPagedList<ProductDto>> GetUsingPagedListAsync(int number, int size,
            Expression<Func<ProductDto, bool>> predicate = null, SortDirection sortDirection = SortDirection.Ascending)
        {
            return await Products.GetUsingPagedListAsync(number, size, predicate).ConfigureAwait(false);
        }

        public async Task<ProductDto> GetAsync(int id)
        {
            return await Products.GetAsync(id).ConfigureAwait(false);
        }

        public async Task<ProductDto> AddAsync(ProductDto product)
        {
            Locker.EnterWriteLock();
            try
            {
                if (await Products.TryAddUniqueProductAsync(product).ConfigureAwait(false))
                {
                    await Products.SaveAsync().ConfigureAwait(false);
                    return await GetAsync(await Products.GetIdAsync(product.Name)
                        .ConfigureAwait(false)).ConfigureAwait(false);
                }
                else
                {
                    throw new ArgumentException("Product already exists!");
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

        public async Task<ProductDto> UpdateAsync(ProductDto product)
        {
            Locker.EnterWriteLock();
            try
            {
                if (await Products.DoesProductExistAsync(product).ConfigureAwait(false))
                    throw new ArgumentException("Product already exists!");

                var result = Products.Update(product);
                await Products.SaveAsync().ConfigureAwait(false);

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
                await Products.DeleteAsync(id).ConfigureAwait(false);
                await Products.SaveAsync().ConfigureAwait(false);
            }
            finally
            {
                if (Locker.IsReadLockHeld)
                {
                    Locker.ExitReadLock();
                }
            }
        }

        public async Task<IEnumerable<ProductDto>> FindAsync(Expression<Func<ProductDto, bool>> predicate)
        {
            return await Products.FindAsync(predicate).ConfigureAwait(false);
        }
    }
}