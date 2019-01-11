using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;
using SalesStatisticsSystem.Contracts.Core.Services;
using SalesStatisticsSystem.Core.Services;
using SalesStatisticsSystem.WebApplication.Models.Filters;
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

        public async Task<ActionResult> Index(SaleFilter saleFilter)
        {
            ViewBag.SaleFilter = new SaleFilter();

            var salesDto = await _saleService.GetAllAsync();

            var salesViewModels = _mapper.Map<IEnumerable<SaleViewModel>>(salesDto);

            return View(salesViewModels);
        }

        public async Task<ActionResult> Find(SaleFilter saleFilter)
        {

            IEnumerable<SaleDto> salesDto;
            if (saleFilter.CustomerFilter.FirstName == null &&
                saleFilter.CustomerFilter.LastName == null &&
                saleFilter.DateFrom == null &&
                saleFilter.DateTo == null &&
                saleFilter.ManagerFilter.LastName == null &&
                saleFilter.ProductFilter.Name == null &&
                saleFilter.SumFrom == null &&
                saleFilter.SumTo == null)
            {
                salesDto = await _saleService.GetAllAsync();
            }
            else
            {
                salesDto = await _saleService.FindAsync(x =>
                    (x.Date >= saleFilter.DateFrom && x.Date <= saleFilter.DateTo) ||
                    (x.Sum >= saleFilter.SumFrom && x.Sum <= saleFilter.SumTo) ||
                    x.Customer.FirstName.Contains(saleFilter.CustomerFilter.FirstName) ||
                    x.Customer.LastName.Contains(saleFilter.CustomerFilter.LastName) ||
                    x.Manager.LastName.Contains(saleFilter.ManagerFilter.LastName) ||
                    x.Product.Name.Contains(saleFilter.ProductFilter.Name));
            }

            var salesViewModels = _mapper.Map<IEnumerable<SaleViewModel>>(salesDto);

            return PartialView("Partial/_SaleTable", salesViewModels);
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

        public async Task<ActionResult> Edit(int id)
        {
            var saleDto = await _saleService.GetAsync(id);

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
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return View();
            }
        }

        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _saleService.DeleteAsync(id);

                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return RedirectToAction("Index");
            }
        }
    }
}