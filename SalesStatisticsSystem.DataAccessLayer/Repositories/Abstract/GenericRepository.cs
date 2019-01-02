using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects.Abstract;
using SalesStatisticsSystem.Contracts.DataAccessLayer.Repositories;
using SalesStatisticsSystem.DataAccessLayer.Support;
using SalesStatisticsSystem.Entity;

namespace SalesStatisticsSystem.DataAccessLayer.Repositories.Abstract
{
    public abstract class GenericRepository<TDto, TEntity> : IGenericRepository<TDto>
         where TDto : DataTransferObject
         where TEntity : class
    {
        protected readonly SalesInformationEntities Context;

        protected readonly IDbSet<TEntity> DbSet;

        protected readonly IMapper Mapper;

        protected GenericRepository(SalesInformationEntities context, IMapper mapper)
        {
            Context = context;

            DbSet = Context.Set<TEntity>();

            Mapper = mapper;
        }

        protected virtual TEntity DtoToEntity(TDto dto)
        {
            return Mapper.Map<TEntity>(dto);
        }

        public void Add(params TDto[] models)
        {
            foreach (var model in models)
            {
                var entity = DtoToEntity(model);

                DbSet.Add(entity);
                Context.Entry(entity).State = EntityState.Added;
            }
        }

        public void Update(params TDto[] models)
        {
            foreach (var model in models)
            {
                var entity = DtoToEntity(model);

                DbSet.Attach(entity);
                Context.Entry(entity).State = EntityState.Modified;
            }
        }

        public void Remove(params TDto[] models)
        {
            foreach (var model in models)
            {
                var entity = DtoToEntity(model);

                if (Context.Entry(entity).State == EntityState.Detached)
                {
                    DbSet.Attach(entity);
                }

                DbSet.Remove(entity);
                Context.Entry(entity).State = EntityState.Deleted;
            }
        }

        public TDto Get(int id)
        {
            return Mapper.Map<TDto>(DbSet.Find(id));
        }

        public IEnumerable<TDto> GetAll()
        {
            return Mapper.Map<IEnumerable<TDto>>(DbSet.AsNoTracking());
        }

        public IEnumerable<TDto> Find(Expression<Func<TDto, bool>> predicate)
        {
            var newPredicate = predicate.Project<TDto, TEntity>();

            return Mapper.Map<IEnumerable<TDto>>(DbSet.AsNoTracking().Where(newPredicate));
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }
}