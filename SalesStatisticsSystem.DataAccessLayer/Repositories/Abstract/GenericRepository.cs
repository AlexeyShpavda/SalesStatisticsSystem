using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Helpers;
using AutoMapper;
using SalesStatisticsSystem.Core.Contracts.Models.Abstract;
using SalesStatisticsSystem.DataAccessLayer.Contracts.Repository.Generic;
using SalesStatisticsSystem.DataAccessLayer.Support.Adapter;
using SalesStatisticsSystem.Entity;
using X.PagedList;

namespace SalesStatisticsSystem.DataAccessLayer.Repositories.Abstract
{
    public abstract class GenericRepository<TDto, TEntity> : IGenericRepository<TDto>
         where TDto : CoreModel
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

        public async Task DeleteAsync(int id)
        {
            var entity = Mapper.Map<TEntity>(await GetAsync(id).ConfigureAwait(false));

            if (Context.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }

           DbSet.Remove(entity);
           Context.Entry(entity).State = EntityState.Deleted;
        }

        public async Task<TDto> GetAsync(int id)
        {
            Expression<Func<TDto, bool>> predicate = x => x.Id == id;

            var newPredicate = predicate.Project<TDto, TEntity>();

            var result = await DbSet.AsNoTracking().FirstOrDefaultAsync(newPredicate).ConfigureAwait(false);

            return Mapper.Map<TDto>(result);
        }

        public async Task<IPagedList<TDto>> GetUsingPagedListAsync(int pageNumber, int pageSize,
            Expression<Func<TDto, bool>> predicate = null, SortDirection sortDirection = SortDirection.Ascending)
        {
            IPagedList<TEntity> result;
            if (predicate != null)
            {
                var newPredicate = predicate.Project<TDto, TEntity>();

                result = await DbSet
                    .AsNoTracking()
                    .Where(newPredicate)
                    .OrderBy("Id", sortDirection)
                    .ToPagedListAsync(pageNumber, pageSize)
                    .ConfigureAwait(false);
            }
            else
            {
                result = await DbSet
                    .AsNoTracking()
                    .OrderBy("Id", sortDirection)
                    .ToPagedListAsync(pageNumber, pageSize)
                    .ConfigureAwait(false);
            }

            return Mapper.Map<IPagedList<TDto>>(result);
        }

        public async Task<IEnumerable<TDto>> FindAsync(Expression<Func<TDto, bool>> predicate)
        {
            var newPredicate = predicate.Project<TDto, TEntity>();

            return Mapper.Map<IEnumerable<TDto>>(await DbSet.AsNoTracking().Where(newPredicate).ToListAsync()
                .ConfigureAwait(false));
        }

        public async Task<int> SaveAsync()
        {
            return await Context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}