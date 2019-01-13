using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Helpers;
using AutoMapper;
using X.PagedList;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;
using SalesStatisticsSystem.Contracts.DataAccessLayer.Repositories;
using SalesStatisticsSystem.DataAccessLayer.Repositories.Abstract;
using SalesStatisticsSystem.DataAccessLayer.Support;
using SalesStatisticsSystem.Entity;

namespace SalesStatisticsSystem.DataAccessLayer.Repositories
{
    public class ProductRepository : GenericRepository<ProductDto, Product>, IProductRepository
    {
        public ProductRepository(SalesInformationEntities context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<bool> TryAddUniqueProductAsync(ProductDto productDto)
        {
            if (await DoesProductExistAsync(productDto).ConfigureAwait(false))
            {
                return false;
            }

            Add(productDto);
            return true;
        }

        public async Task<int> GetIdAsync(string productName)
        {
            Expression<Func<ProductDto, bool>> predicate = x => x.Name == productName;

            var result = await FindAsync(predicate).ConfigureAwait(false);

            return result.First().Id;
        }

        public async Task<bool> DoesProductExistAsync(ProductDto productDto)
        {
            Expression<Func<ProductDto, bool>> predicate = x => x.Name == productDto.Name;

            var result = await FindAsync(predicate).ConfigureAwait(false);

            return result.Any();
        }
    }
}