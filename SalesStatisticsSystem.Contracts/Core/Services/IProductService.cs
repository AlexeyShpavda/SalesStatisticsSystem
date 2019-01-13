﻿using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;
using X.PagedList;

namespace SalesStatisticsSystem.Contracts.Core.Services
{
    public interface IProductService : IGenericService<ProductDto>
    {
        Task<IPagedList<ProductDto>> GetUsingPagedListAsync(int number, int size,
            Expression<Func<ProductDto, bool>> predicate = null);
    }
}