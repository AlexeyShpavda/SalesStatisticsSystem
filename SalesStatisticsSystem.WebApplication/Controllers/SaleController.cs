using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;
using SalesStatisticsSystem.Contracts.Core.Services;
using SalesStatisticsSystem.WebApplication.Models.Filters;
using SalesStatisticsSystem.WebApplication.Models.SaleViewModels;

namespace SalesStatisticsSystem.WebApplication.Controllers
{
    public class SaleController : Controller
    {
        private readonly ISaleService _saleService;

        private readonly IMapper _mapper;

        public SaleController(ISaleService saleService, IMapper mapper)
        {
            _saleService = saleService;

            _mapper = mapper;
        }

        public async Task<ActionResult> Index(SaleFilter saleFilter)
        {
            try
            {
                var salesDto = await _saleService.GetAllAsync().ConfigureAwait(false);

                var salesViewModels = _mapper.Map<IEnumerable<SaleViewModel>>(salesDto).ToList();

                #region Chart
                var uniqueProducts = salesViewModels.Select(x => x.Product.Name).Distinct();

                var productSalesQuantity = uniqueProducts
                    .Select(product => salesViewModels.Count(x => x.Product.Name == product)).ToList();

                ViewBag.UniqueProducts = uniqueProducts;
                ViewBag.ProductSalesQuantity = productSalesQuantity;
                #endregion

                ViewBag.SaleFilter = new SaleFilter();

                return View(salesViewModels);
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return View();
            }
        }

        public async Task<ActionResult> Find(SaleFilter saleFilter)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var dto = await _saleService.GetAllAsync().ConfigureAwait(false);

                    var viewModels = _mapper.Map<IEnumerable<SaleViewModel>>(dto);

                    return PartialView("Partial/_SaleTable", viewModels);
                }

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
                    salesDto = await _saleService.GetAllAsync().ConfigureAwait(false);
                }
                else
                {
                    salesDto = await _saleService.FindAsync(x =>
                        (x.Date >= saleFilter.DateFrom && x.Date <= saleFilter.DateTo) ||
                        (x.Sum >= saleFilter.SumFrom && x.Sum <= saleFilter.SumTo) ||
                        x.Customer.FirstName.Contains(saleFilter.CustomerFilter.FirstName) ||
                        x.Customer.LastName.Contains(saleFilter.CustomerFilter.LastName) ||
                        x.Manager.LastName.Contains(saleFilter.ManagerFilter.LastName) ||
                        x.Product.Name.Contains(saleFilter.ProductFilter.Name)).ConfigureAwait(false);
                }

                var salesViewModels = _mapper.Map<IEnumerable<SaleViewModel>>(salesDto);

                return PartialView("Partial/_SaleTable", salesViewModels);
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return PartialView("Partial/_SaleTable");
            }

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

                await _saleService.AddAsync(_mapper.Map<SaleDto>(sale)).ConfigureAwait(false);

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
            try
            {
                var saleDto = await _saleService.GetAsync(id).ConfigureAwait(false);

                var saleViewModel = _mapper.Map<SaleViewModel>(saleDto);

                return View(saleViewModel);
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return View();
            }
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

                await _saleService.UpdateAsync(_mapper.Map<SaleDto>(sale)).ConfigureAwait(false);

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
                await _saleService.DeleteAsync(id).ConfigureAwait(false);

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