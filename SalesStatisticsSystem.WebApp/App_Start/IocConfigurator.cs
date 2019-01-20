using System.Web.Mvc;
using SalesStatisticsSystem.Core.Contracts.Services;
using SalesStatisticsSystem.Core.Services;
using SalesStatisticsSystem.WebApp.Infrastructure;
using Unity;
using Unity.Lifetime;

namespace SalesStatisticsSystem.WebApp
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
            unityContainer.RegisterType<ICustomerService, CustomerService>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<IProductService, ProductService>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<ISaleService, SaleService>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<IManagerService, ManagerService>(new ContainerControlledLifetimeManager());

            unityContainer.RegisterInstance(Support.Adapter.AutoMapper.CreateConfiguration().CreateMapper());
        }
    }
}