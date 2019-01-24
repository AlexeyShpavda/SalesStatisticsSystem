using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using SalesStatisticsSystem.Core.Contracts.Models.Sales;
using SalesStatisticsSystem.DataAccessLayer.Contracts.Repository;
using SalesStatisticsSystem.DataAccessLayer.Repositories.Abstract;
using SalesStatisticsSystem.Entity;

namespace SalesStatisticsSystem.DataAccessLayer.Repositories
{
    public class ProductRepository : GenericRepository<ProductCoreModel, Product>, IProductRepository
    {
        public ProductRepository(SalesInformationEntities context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<bool> TryAddUniqueProductAsync(ProductCoreModel productCoreModel)
        {
            if (await DoesProductExistAsync(productCoreModel).ConfigureAwait(false))
            {
                return false;
            }

            Add(productCoreModel);
            return true;
        }

        public async Task<int> GetIdAsync(string productName)
        {
            Expression<Func<ProductCoreModel, bool>> predicate = x => x.Name == productName;

            var result = await FindAsync(predicate).ConfigureAwait(false);

            return result.First().Id;
        }

        public async Task<bool> DoesProductExistAsync(ProductCoreModel productCoreModel)
        {
            Expression<Func<ProductCoreModel, bool>> predicate = x => x.Name == productCoreModel.Name;

            var result = await FindAsync(predicate).ConfigureAwait(false);

            return result.Any();
        }
    }
}