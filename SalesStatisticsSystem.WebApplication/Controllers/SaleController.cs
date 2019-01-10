using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;
using SalesStatisticsSystem.Contracts.Core.Services;
using SalesStatisticsSystem.Core.Services;
using SalesStatisticsSystem.WebApplication.Models.SaleViewModels;

namespace SalesStatisticsSystem.WebApplication.Controllers
{
    public class SaleController : Controller
    {
        private readonly ISaleService _saleService;

        private readonly IMapper _mapper;

        public SaleController()
        {
            _mapper = Support.Adapter.AutoMapper.CreateConfiguration().CreateMapper();

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
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(SaleViewModel sale)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(sale);
                }

                await _saleService.AddAsync(_mapper.Map<SaleDto>(sale));

                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            var saleDto = _saleService.GetAsync(id);

            var saleViewModel = _mapper.Map<SaleViewModel>(saleDto);

            return View(saleViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(SaleViewModel sale)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(sale);
                }

                await _saleService.UpdateAsync(_mapper.Map<SaleDto>(sale));

                return RedirectToAction("Index");
            }
            catch(Exception exception)
            {
                ViewBag.Error = exception.Message;

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