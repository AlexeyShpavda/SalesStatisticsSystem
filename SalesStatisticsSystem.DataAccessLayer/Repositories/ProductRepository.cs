using System;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;
using SalesStatisticsSystem.DataAccessLayer.Repositories.Abstract;
using SalesStatisticsSystem.Entity;

namespace SalesStatisticsSystem.DataAccessLayer.Repositories
{
    public class ProductRepository : GenericRepository<ProductDto, Product>, IProductRepository
    {
        public ProductRepository(SalesInformationEntities context, IMapper mapper) : base(context, mapper)
        {
        }

        public void AddUniqueProductToDatabase(ProductDto productDto)
        {
            Expression<Func<ProductDto, bool>> predicate = x => x.Name == productDto.Name;

            if (Find(predicate).Any()) return;

            Add(productDto);
        }

        public int? GetId(string productName)
        {
            Expression<Func<ProductDto, bool>> predicate = x => x.Name == productName;

            return Find(predicate).First().Id;
        }
    }
}