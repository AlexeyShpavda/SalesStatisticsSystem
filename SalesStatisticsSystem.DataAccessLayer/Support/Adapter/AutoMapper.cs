using System.Collections.Generic;
using AutoMapper;
using SalesStatisticsSystem.Core.Contracts.Models;
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
                config.CreateMap<Customer, CustomerCoreModel>();
                config.CreateMap<CustomerCoreModel, Customer>();

                config.CreateMap<Product, ProductCoreModel>();
                config.CreateMap<ProductCoreModel, Product>();

                config.CreateMap<Manager, ManagerCoreModel>();
                config.CreateMap<ManagerCoreModel, Manager>();

                config.CreateMap<Sale, SaleCoreModel>();
                config.CreateMap<SaleCoreModel, Sale>()
                    .ForMember(x => x.Customer, opt => opt.Ignore())
                    .ForMember(x => x.Manager, opt => opt.Ignore())
                    .ForMember(x => x.Product, opt => opt.Ignore());

                config
                    .CreateMap<IPagedList<Customer>, IPagedList<CustomerCoreModel>>()
                    .ConvertUsing<PagedListConverter<Customer, CustomerCoreModel>>();
                config
                    .CreateMap<IPagedList<CustomerCoreModel>, IPagedList<Customer>>()
                    .ConvertUsing<PagedListConverter<CustomerCoreModel, Customer>>();

                config
                    .CreateMap<IPagedList<Product>, IPagedList<ProductCoreModel>>()
                    .ConvertUsing<PagedListConverter<Product, ProductCoreModel>>();
                config
                    .CreateMap<IPagedList<ProductCoreModel>, IPagedList<Product>>()
                    .ConvertUsing<PagedListConverter<ProductCoreModel, Product>>();

                config
                    .CreateMap<IPagedList<Manager>, IPagedList<ManagerCoreModel>>()
                    .ConvertUsing<PagedListConverter<Manager, ManagerCoreModel>>();
                config
                    .CreateMap<IPagedList<ManagerCoreModel>, IPagedList<Manager>>()
                    .ConvertUsing<PagedListConverter<ManagerCoreModel, Manager>>();

                config
                    .CreateMap<IPagedList<Sale>, IPagedList<SaleCoreModel>>()
                    .ConvertUsing<PagedListConverter<Sale, SaleCoreModel>>();
                config
                    .CreateMap<IPagedList<SaleCoreModel>, IPagedList<Sale>>()
                    .ConvertUsing<PagedListConverter<SaleCoreModel, Sale>>();
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