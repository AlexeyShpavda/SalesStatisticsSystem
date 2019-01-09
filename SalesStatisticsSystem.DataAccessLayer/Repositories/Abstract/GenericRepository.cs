using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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

        public TDto Add(TDto model)
        {
            var entity = DtoToEntity(model);

            var result = DbSet.Add(entity);
            Context.Entry(entity).State = EntityState.Added;

            return Mapper.Map<TDto>(result);
        }

        public TDto Update(TDto model)
        {
            var entity = DtoToEntity(model);

            var result = DbSet.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;

            return Mapper.Map<TDto>(result);
        }

        public void Remove(int id)
        {
            var entity = DbSet.Find(id);

            if (Context.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }

           DbSet.Remove(entity);
           Context.Entry(entity).State = EntityState.Deleted;
        }

        public TDto Get(int id)
        {
            // TODO: Working with nullable types in Expression Trees

            //Expression<Func<TDto, bool>> predicate = x => x.Id.Value == id;

            //var newPredicate = predicate.Project<TDto, TEntity>();

            //var result = await DbSet.FirstOrDefaultAsync(newPredicate);

            //return Mapper.Map<TDto>(result);

            return Mapper.Map<TDto>(DbSet.Find(id));
        }

        public async Task<IEnumerable<TDto>> GetAllAsync()
        {
            var result = await DbSet.AsNoTracking().ToListAsync();

            return Mapper.Map<IEnumerable<TDto>>(result);
        }

        public IEnumerable<TDto> Find(Expression<Func<TDto, bool>> predicate)
        {
            var newPredicate = predicate.Project<TDto, TEntity>();

            return Mapper.Map<IEnumerable<TDto>>(DbSet.AsNoTracking().Where(newPredicate));
        }

        public async Task<int> SaveAsync()
        {
            return await Context.SaveChangesAsync();
        }
    }
}