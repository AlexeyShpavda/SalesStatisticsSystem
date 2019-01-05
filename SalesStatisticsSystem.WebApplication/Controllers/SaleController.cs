using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using SalesStatisticsSystem.Core.Services;
using SalesStatisticsSystem.WebApplication.Models;

namespace SalesStatisticsSystem.WebApplication.Controllers
{
    public class SaleController : Controller
    {
        private readonly SaleService _saleService;

        private readonly IMapper _mapper;

        public SaleController()
        {
            _mapper = Support.AutoMapper.CreateConfiguration().CreateMapper();

            _saleService = new SaleService();
        }

        public async Task<ActionResult> Index()
        {
            var sales = await _saleService.GetSalesAsync();

            var salesViewModels = _mapper.Map<IEnumerable<SaleViewModel>>(sales);

            return View(salesViewModels);
        }
    }
}