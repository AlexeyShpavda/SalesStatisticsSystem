using System.Collections.Generic;
using AutoMapper;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;
using SalesStatisticsSystem.Entity;
using X.PagedList;

namespace SalesStatisticsSystem.DataAccessLayer.Support.Adapter
{
    internal static class AutoMapper
    {
        internal static MapperConfiguration CreateConfiguration()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<Customer, CustomerDto>();
                config.CreateMap<CustomerDto, Customer>();

                config.CreateMap<Product, ProductDto>();
                config.CreateMap<ProductDto, Product>();

                config.CreateMap<Manager, ManagerDto>();
                config.CreateMap<ManagerDto, Manager>();

                config.CreateMap<Sale, SaleDto>();
                config.CreateMap<SaleDto, Sale>()
                    .ForMember(x => x.Customer, opt => opt.Ignore())
                    .ForMember(x => x.Manager, opt => opt.Ignore())
                    .ForMember(x => x.Product, opt => opt.Ignore());

                config
                    .CreateMap<IPagedList<Customer>, IPagedList<CustomerDto>>()
                    .ConvertUsing<PagedListConverter<Customer, CustomerDto>>();
                config
                    .CreateMap<IPagedList<CustomerDto>, IPagedList<Customer>>()
                    .ConvertUsing<PagedListConverter<CustomerDto, Customer>>();

                config
                    .CreateMap<IPagedList<Product>, IPagedList<ProductDto>>()
                    .ConvertUsing<PagedListConverter<Product, ProductDto>>();
                config
                    .CreateMap<IPagedList<ProductDto>, IPagedList<Product>>()
                    .ConvertUsing<PagedListConverter<ProductDto, Product>>();

                config
                    .CreateMap<IPagedList<Manager>, IPagedList<ManagerDto>>()
                    .ConvertUsing<PagedListConverter<Manager, ManagerDto>>();
                config
                    .CreateMap<IPagedList<ManagerDto>, IPagedList<Manager>>()
                    .ConvertUsing<PagedListConverter<ManagerDto, Manager>>();

                config
                    .CreateMap<IPagedList<Sale>, IPagedList<SaleDto>>()
                    .ConvertUsing<PagedListConverter<Sale, SaleDto>>();
                config
                    .CreateMap<IPagedList<SaleDto>, IPagedList<Sale>>()
                    .ConvertUsing<PagedListConverter<SaleDto, Sale>>();
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