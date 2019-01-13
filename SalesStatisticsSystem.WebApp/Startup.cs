using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SalesStatisticsSystem.WebApp.Startup))]
namespace SalesStatisticsSystem.WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
