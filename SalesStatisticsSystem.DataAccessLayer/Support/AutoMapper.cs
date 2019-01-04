using AutoMapper;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;
using SalesStatisticsSystem.Entity;

namespace SalesStatisticsSystem.DataAccessLayer.Support
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
            });
        }
    }
}