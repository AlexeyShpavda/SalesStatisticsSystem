using System.Web.Mvc;
using SalesStatisticsSystem.Contracts.Core.Services;
using SalesStatisticsSystem.Core.Services;
using SalesStatisticsSystem.WebApplication.Infrastructure;
using Unity;

namespace SalesStatisticsSystem.WebApplication
{
    public static class IocConfigurator
    {
        public static void ConfigureIocUnityContainer()
        {
            IUnityContainer unityContainer = new UnityContainer();

            RegisterServices(unityContainer);

            DependencyResolver.SetResolver(new UnitDependencyResolver(unityContainer));
        }

        private static void RegisterServices(IUnityContainer unityContainer)
        {
            unityContainer.RegisterType<ICustomerService, CustomerService>();
            unityContainer.RegisterType<IProductService, ProductService>();
            unityContainer.RegisterType<ISaleService, SaleService>();
            unityContainer.RegisterType<IManagerService, ManagerService>();
            unityContainer.RegisterInstance(Support.Adapter.AutoMapper.CreateConfiguration().CreateMapper());
        }
    }
}