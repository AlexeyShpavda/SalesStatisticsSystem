﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;
using SalesStatisticsSystem.Contracts.DataAccessLayer.Repositories;
using SalesStatisticsSystem.Contracts.DataAccessLayer.UnitOfWorks;
using SalesStatisticsSystem.DataAccessLayer.Repositories;
using SalesStatisticsSystem.Entity;

namespace SalesStatisticsSystem.DataAccessLayer.UnitOfWorks
{
    public class SaleUnitOfWork : ISaleUnitOfWork
    {
        private SalesInformationEntities Context { get; }
        private ReaderWriterLockSlim Locker { get; }

        private ICustomerRepository Customers { get; }
        private IManagerRepository Managers { get; }
        private IProductRepository Products { get; }
        private ISaleRepository Sales { get; }

        public SaleUnitOfWork(SalesInformationEntities context, ReaderWriterLockSlim locker)
        {
            Context = context;
            Locker = locker;

            var mapper = Support.AutoMapper.CreateConfiguration().CreateMapper();
            Customers = new CustomerRepository(Context, mapper);
            Managers = new ManagerRepository(Context, mapper);
            Products = new ProductRepository(Context, mapper);
            Sales = new SaleRepository(Context, mapper);
        }

        public void Add(params SaleDto[] sales)
        {
            Locker.EnterWriteLock();
            try
            {
                foreach (var sale in sales)
                {
                    Customers.AddUniqueCustomerToDatabase(sale.Customer);
                    Customers.Save();
                    sale.Customer.Id = Customers.GetId(sale.Customer.FirstName, sale.Customer.LastName);

                    Managers.AddUniqueManagerToDatabase(sale.Manager);
                    Managers.Save();
                    sale.Manager.Id = Managers.GetId(sale.Manager.LastName);

                    Products.AddUniqueProductToDatabase(sale.Product);
                    Products.Save();
                    sale.Product.Id = Products.GetId(sale.Product.Name);

                    Sales.Add(sale);
                    Sales.Save();
                }
            }
            finally
            {
                Locker.ExitWriteLock();
            }
        }

        public async Task<IEnumerable<SaleDto>> GetAsync()
        {
            return await Sales.GetAllAsync();
        }
    }
}