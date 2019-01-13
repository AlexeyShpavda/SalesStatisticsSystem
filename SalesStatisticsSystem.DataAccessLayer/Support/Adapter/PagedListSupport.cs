using System.Collections.Generic;
using AutoMapper;
using X.PagedList;

namespace SalesStatisticsSystem.DataAccessLayer.Support.Adapter
{
    public static class PagedListSupport
    {
        // Сan be Used Instead of AutoMapper
        public static IPagedList<TDestination> ToMappedPagedList<TSource, TDestination>(this IPagedList<TSource> list)
        {
            var sourceList = Mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(list);
            IPagedList<TDestination> pagedResult = new StaticPagedList<TDestination>(sourceList, list.GetMetaData());
            return pagedResult;
        }
    }
}