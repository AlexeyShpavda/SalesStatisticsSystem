using System.Threading.Tasks;
using System.Web.Mvc;
using SalesStatisticsSystem.Core.Services;

namespace SalesStatisticsSystem.WebApplication.Controllers
{
    public class SaleController : Controller
    {
        private readonly SaleService _saleService;

        public SaleController()
        {
            _saleService = new SaleService();
        }

        public async Task<ActionResult> Index()
        {
            var sales = await _saleService.GetSalesAsync();

            return View(sales);
        }
    }
}