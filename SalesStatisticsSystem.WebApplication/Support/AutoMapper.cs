using AutoMapper;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;
using SalesStatisticsSystem.WebApplication.Models.SaleViewModels;

namespace SalesStatisticsSystem.WebApplication.Support
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
            });
        }
    }
}