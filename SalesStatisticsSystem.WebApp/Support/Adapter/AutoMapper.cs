using System.Collections.Generic;
using AutoMapper;
using SalesStatisticsSystem.Core.Contracts.Models.Filters;
using SalesStatisticsSystem.Core.Contracts.Models.Sales;
using SalesStatisticsSystem.WebApp.Models.Filters;
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
                config.CreateMap<CustomerViewModel, CustomerCoreModel>();
                config.CreateMap<CustomerCoreModel, CustomerViewModel>();

                config.CreateMap<ProductViewModel, ProductCoreModel>();
                config.CreateMap<ProductCoreModel, ProductViewModel>();

                config.CreateMap<ManagerViewModel, ManagerCoreModel>();
                config.CreateMap<ManagerCoreModel, ManagerViewModel>();

                config.CreateMap<SaleViewModel, SaleCoreModel>();
                config.CreateMap<SaleCoreModel, SaleViewModel>();

                config.CreateMap<CustomerFilterCoreModel, CustomerFilterViewModel>();
                config.CreateMap<CustomerFilterViewModel, CustomerFilterCoreModel>();

                config.CreateMap<ProductFilterCoreModel, ProductFilterViewModel>();
                config.CreateMap<ProductFilterViewModel, ProductFilterCoreModel>();

                config.CreateMap<ManagerFilterViewModel, ManagerFilterCoreModel>();
                config.CreateMap<ManagerFilterCoreModel, ManagerFilterViewModel>();

                config.CreateMap<SaleFilterViewModel, SaleFilterCoreModel>();
                config.CreateMap<SaleFilterCoreModel, SaleFilterViewModel>();

                config
                    .CreateMap<IPagedList<CustomerViewModel>, IPagedList<CustomerCoreModel>>()
                    .ConvertUsing<PagedListConverter<CustomerViewModel, CustomerCoreModel>>();
                config
                    .CreateMap<IPagedList<CustomerCoreModel>, IPagedList<CustomerViewModel>>()
                    .ConvertUsing<PagedListConverter<CustomerCoreModel, CustomerViewModel>>();

                config
                    .CreateMap<IPagedList<ProductViewModel>, IPagedList<ProductCoreModel>>()
                    .ConvertUsing<PagedListConverter<ProductViewModel, ProductCoreModel>>();
                config
                    .CreateMap<IPagedList<ProductCoreModel>, IPagedList<ProductViewModel>>()
                    .ConvertUsing<PagedListConverter<ProductCoreModel, ProductViewModel>>();

                config
                    .CreateMap<IPagedList<ManagerViewModel>, IPagedList<ManagerCoreModel>>()
                    .ConvertUsing<PagedListConverter<ManagerViewModel, ManagerCoreModel>>();
                config
                    .CreateMap<IPagedList<ManagerCoreModel>, IPagedList<ManagerViewModel>>()
                    .ConvertUsing<PagedListConverter<ManagerCoreModel, ManagerViewModel>>();

                config
                    .CreateMap<IPagedList<SaleViewModel>, IPagedList<SaleCoreModel>>()
                    .ConvertUsing<PagedListConverter<SaleViewModel, SaleCoreModel>>();
                config
                    .CreateMap<IPagedList<SaleCoreModel>, IPagedList<SaleViewModel>>()
                    .ConvertUsing<PagedListConverter<SaleCoreModel, SaleViewModel>>();
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