using System.Collections.Generic;
using AutoMapper;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;
using SalesStatisticsSystem.WebApp.Models.SaleViewModels;
using X.PagedList;

namespace SalesStatisticsSystem.WebApp.Support.Adapter
{
    internal static class AutoMapper
    {
        internal static MapperConfiguration CreateConfiguration()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<CustomerViewModel, CustomerDto>();
                config.CreateMap<CustomerDto, CustomerViewModel>();

                config.CreateMap<ProductViewModel, ProductDto>();
                config.CreateMap<ProductDto, ProductViewModel>();

                config.CreateMap<ManagerViewModel, ManagerDto>();
                config.CreateMap<ManagerDto, ManagerViewModel>();

                config.CreateMap<SaleViewModel, SaleDto>();
                config.CreateMap<SaleDto, SaleViewModel>();

                config
                    .CreateMap<IPagedList<CustomerViewModel>, IPagedList<CustomerDto>>()
                    .ConvertUsing<PagedListConverter<CustomerViewModel, CustomerDto>>();
                config
                    .CreateMap<IPagedList<CustomerDto>, IPagedList<CustomerViewModel>>()
                    .ConvertUsing<PagedListConverter<CustomerDto, CustomerViewModel>>();

                config
                    .CreateMap<IPagedList<ProductViewModel>, IPagedList<ProductDto>>()
                    .ConvertUsing<PagedListConverter<ProductViewModel, ProductDto>>();
                config
                    .CreateMap<IPagedList<ProductDto>, IPagedList<ProductViewModel>>()
                    .ConvertUsing<PagedListConverter<ProductDto, ProductViewModel>>();

                config
                    .CreateMap<IPagedList<ManagerViewModel>, IPagedList<ManagerDto>>()
                    .ConvertUsing<PagedListConverter<ManagerViewModel, ManagerDto>>();
                config
                    .CreateMap<IPagedList<ManagerDto>, IPagedList<ManagerViewModel>>()
                    .ConvertUsing<PagedListConverter<ManagerDto, ManagerViewModel>>();

                config
                    .CreateMap<IPagedList<SaleViewModel>, IPagedList<SaleDto>>()
                    .ConvertUsing<PagedListConverter<SaleViewModel, SaleDto>>();
                config
                    .CreateMap<IPagedList<SaleDto>, IPagedList<SaleViewModel>>()
                    .ConvertUsing<PagedListConverter<SaleDto, SaleViewModel>>();
            });
        }
    }

    public class PagedListConverter<TSource, TDestination>
        : ITypeConverter<IPagedList<TSource>, IPagedList<TDestination>>
        where TSource : class
        where TDestination : class
    {
        public IPagedList<TDestination> Convert(IPagedList<TSource> source, IPagedList<TDestination> destination,
            ResolutionContext context)
        {
            var sourceList = context.Mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(source);
            IPagedList<TDestination> pagedResult = new StaticPagedList<TDestination>(sourceList, source.GetMetaData());
            return pagedResult;
        }
    }
}