using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;
using SalesStatisticsSystem.Core.Contracts.Models.Filters;
using SalesStatisticsSystem.Core.Contracts.Models.Sales;
using SalesStatisticsSystem.Core.Contracts.Services;
using SalesStatisticsSystem.DataAccessLayer.Contracts.ReaderWriter;
using SalesStatisticsSystem.DataAccessLayer.ReaderWriter;
using SalesStatisticsSystem.Entity;
using X.PagedList;

namespace SalesStatisticsSystem.Core.Services
{
    public class SaleService : ISaleService, IDisposable
    {
        private SalesInformationEntities Context { get; }

        private ReaderWriterLockSlim Locker { get; }

        private ISaleDbReaderWriter SaleDbReaderWriter { get; }

        public SaleService()
        {
            Context = new SalesInformationEntities();

            Locker = new ReaderWriterLockSlim();

            SaleDbReaderWriter = new SaleDbReaderWriter(Context, Locker);
        }

        public async Task<IPagedList<SaleCoreModel>> GetUsingPagedListAsync(int pageNumber, int pageSize,
            Expression<Func<SaleCoreModel, bool>> predicate = null, SortDirection sortDirection = SortDirection.Ascending)
        {
            return await SaleDbReaderWriter.GetUsingPagedListAsync(pageNumber, pageSize, predicate)
                .ConfigureAwait(false);
        }

        public async Task<IPagedList<SaleCoreModel>> Filter(SaleFilterCoreModel saleFilterCoreModel,
            int pageSize, SortDirection sortDirection = SortDirection.Ascending)
        {
            if (saleFilterCoreModel.CustomerFirstName == null &&
                saleFilterCoreModel.CustomerLastName == null &&
                saleFilterCoreModel.DateFrom == null &&
                saleFilterCoreModel.DateTo == null &&
                saleFilterCoreModel.ManagerLastName == null &&
                saleFilterCoreModel.ProductName == null &&
                saleFilterCoreModel.SumFrom == null &&
                saleFilterCoreModel.SumTo == null)
            {
                return await GetUsingPagedListAsync(saleFilterCoreModel.Page ?? 1, pageSize)
                    .ConfigureAwait(false);
            }

            return await GetUsingPagedListAsync(
                saleFilterCoreModel.Page ?? 1, pageSize, x =>
                    (x.Date >= saleFilterCoreModel.DateFrom && x.Date <= saleFilterCoreModel.DateTo) &&
                    (x.Sum >= saleFilterCoreModel.SumFrom && x.Sum <= saleFilterCoreModel.SumTo) &&
                    x.Customer.FirstName.Contains(saleFilterCoreModel.CustomerFirstName) &&
                    x.Customer.LastName.Contains(saleFilterCoreModel.CustomerLastName) &&
                    x.Manager.LastName.Contains(saleFilterCoreModel.ManagerLastName) &&
                    x.Product.Name.Contains(saleFilterCoreModel.ProductName)).ConfigureAwait(false);

        }

        public async Task<SaleCoreModel> GetAsync(int id)
        {
            return await SaleDbReaderWriter.GetAsync(id).ConfigureAwait(false);
        }

        public async Task<SaleCoreModel> AddAsync(SaleCoreModel model)
        {
            return await SaleDbReaderWriter.AddAsync(model).ConfigureAwait(false);
        }

        public async Task<SaleCoreModel> UpdateAsync(SaleCoreModel model)
        {
            return await SaleDbReaderWriter.UpdateAsync(model).ConfigureAwait(false);
        }

        public async Task DeleteAsync(int id)
        {
            await SaleDbReaderWriter.DeleteAsync(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<SaleCoreModel>> FindAsync(Expression<Func<SaleCoreModel, bool>> predicate)
        {
            return await SaleDbReaderWriter.FindAsync(predicate).ConfigureAwait(false);
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