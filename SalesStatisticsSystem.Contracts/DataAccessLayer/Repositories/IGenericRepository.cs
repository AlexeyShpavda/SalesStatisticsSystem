using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SalesStatisticsSystem.Contracts.DataAccessLayer.Repositories
{
    public interface IGenericRepository<TDto> where TDto : DataTransferObject
    {
        void Add(params TDto[] models);

        void Update(params TDto[] entities);

        void Remove(params TDto[] entities);

        TDto Get(int id);

        IEnumerable<TDto> GetAll();

        IEnumerable<TDto> Find(Expression<Func<TDto, bool>> predicate);

        void Save();
    }
}