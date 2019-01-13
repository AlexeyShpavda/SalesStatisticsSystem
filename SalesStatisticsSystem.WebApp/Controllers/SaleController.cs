using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;
using SalesStatisticsSystem.Contracts.Core.Services;
using SalesStatisticsSystem.WebApp.Models.Filters;
using SalesStatisticsSystem.WebApp.Models.SaleViewModels;

namespace SalesStatisticsSystem.WebApp.Controllers
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

        public async Task<ActionResult> Index()
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

                ViewBag.SaleFilter = new SaleFilterModel();

                return View(salesViewModels);
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return View();
            }
        }

        public async Task<ActionResult> Find(SaleFilterModel saleFilterModel)
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
                if (saleFilterModel.CustomerFilter.FirstName == null &&
                    saleFilterModel.CustomerFilter.LastName == null &&
                    saleFilterModel.DateFrom == null &&
                    saleFilterModel.DateTo == null &&
                    saleFilterModel.ManagerFilter.LastName == null &&
                    saleFilterModel.ProductFilter.Name == null &&
                    saleFilterModel.SumFrom == null &&
                    saleFilterModel.SumTo == null)
                {
                    salesDto = await _saleService.GetAllAsync().ConfigureAwait(false);
                }
                else
                {
                    salesDto = await _saleService.FindAsync(x =>
                        (x.Date >= saleFilterModel.DateFrom && x.Date <= saleFilterModel.DateTo) ||
                        (x.Sum >= saleFilterModel.SumFrom && x.Sum <= saleFilterModel.SumTo) ||
                        x.Customer.FirstName.Contains(saleFilterModel.CustomerFilter.FirstName) ||
                        x.Customer.LastName.Contains(saleFilterModel.CustomerFilter.LastName) ||
                        x.Manager.LastName.Contains(saleFilterModel.ManagerFilter.LastName) ||
                        x.Product.Name.Contains(saleFilterModel.ProductFilter.Name)).ConfigureAwait(false);
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