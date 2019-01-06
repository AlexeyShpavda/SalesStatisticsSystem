using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;
using SalesStatisticsSystem.Contracts.Core.Services;
using SalesStatisticsSystem.Core.Services;
using SalesStatisticsSystem.WebApplication.Models;

namespace SalesStatisticsSystem.WebApplication.Controllers
{
    public class SaleController : Controller
    {
        private readonly ISaleService _saleService;

        private readonly IMapper _mapper;

        public SaleController()
        {
            _mapper = Support.AutoMapper.CreateConfiguration().CreateMapper();

            _saleService = new SaleService();
        }

        public async Task<ActionResult> Index()
        {
            var salesDto = await _saleService.GetAllAsync();

            var salesViewModels = _mapper.Map<IEnumerable<SaleViewModel>>(salesDto);

            return View(salesViewModels);
        }

        public ActionResult Create()
        {
            return View(new SaleViewModel {Date = DateTime.Now});
        }

        [HttpPost]
        public ActionResult Create(SaleViewModel sale)
        {
            try
            {
                _saleService.Add(_mapper.Map<SaleDto>(sale));

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                _saleService.Delete(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
    }
}