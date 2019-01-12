using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Unity;

namespace SalesStatisticsSystem.WebApplication.Infrastructure
{
    public class UnitDependencyResolver : IDependencyResolver
    {
        private readonly IUnityContainer _unityContainer;

        public UnitDependencyResolver(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return _unityContainer.Resolve(serviceType);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return _unityContainer.ResolveAll(serviceType);
            }
            catch (Exception)
            {
                return new List<object>();
            }
        }
    }
}