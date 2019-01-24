using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;
using AutoMapper;
using SalesStatisticsSystem.Core.Contracts.Models.Filters;
using SalesStatisticsSystem.Core.Contracts.Models.Sales;
using SalesStatisticsSystem.Core.Contracts.Services;
using SalesStatisticsSystem.WebApp.Models.Filters;
using SalesStatisticsSystem.WebApp.Models.SaleViewModels;
using X.PagedList;

namespace SalesStatisticsSystem.WebApp.Controllers
{
    [Authorize]
    public class SaleController : Controller
    {
        private readonly ISaleService _saleService;

        private readonly IMapper _mapper;

        private readonly int _pageSize;

        private readonly int _numberOfRecordsToCreateSchedule;

        public SaleController(ISaleService saleService, IMapper mapper)
        {
            _saleService = saleService;

            _mapper = mapper;

            _pageSize = int.Parse(ConfigurationManager.AppSettings["numberOfRecordsPerPage"]);

            _numberOfRecordsToCreateSchedule =
                int.Parse(ConfigurationManager.AppSettings["numberOfRecordsToCreateSchedule"]);
        }

        [HttpGet]
        public async Task<ActionResult> Index(int? page)
        {
            try
            {
                ViewBag.SaleFilter = new SaleFilterViewModel();

                var salesCoreModels = await _saleService.GetUsingPagedListAsync(page ?? 1, _pageSize);

                var salesViewModels =
                        _mapper.Map<IPagedList<SaleViewModel>>(salesCoreModels);

                return View(salesViewModels);
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return View();
            }
        }

        [HttpGet]
        public async Task<ActionResult> ShowGraph(int? page)
        {
            try
            {
                var salesForGraphCoreModels =
                    await _saleService.GetUsingPagedListAsync(page ?? 1, _numberOfRecordsToCreateSchedule, null,
                        SortDirection.Descending);

                var salesForGraphViewModels =
                    _mapper.Map<IEnumerable<SaleViewModel>>(salesForGraphCoreModels).ToList();

                var uniqueProducts = salesForGraphViewModels.Select(x => x.Product.Name).Distinct();

                var productSalesQuantity = uniqueProducts
                    .Select(product => salesForGraphViewModels.Count(x => x.Product.Name == product));

                ViewBag.UniqueProducts = uniqueProducts;
                ViewBag.ProductSalesQuantity = productSalesQuantity;

                return PartialView("~/Views/Sale/Partial/_Chart.cshtml");
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return View("Index");
            }
        }

        [HttpGet]
        public async Task<ActionResult> Find(SaleFilterViewModel saleFilterViewModel)
        {
            try
            {
                #region Validation
                if (!ModelState.IsValid)
                {
                    var coreModels = await _saleService.GetUsingPagedListAsync(saleFilterViewModel.Page ?? 1, _pageSize)
                        .ConfigureAwait(false);

                    var viewModels = _mapper.Map<IPagedList<SaleViewModel>>(coreModels);

                    return PartialView("Partial/_SaleTable", viewModels);
                }
                #endregion

                # region Filter

                var salesCoreModels = _saleService.Filter(
                    _mapper.Map<SaleFilterCoreModel>(saleFilterViewModel), _pageSize);

                var salesViewModels = _mapper.Map<IPagedList<SaleViewModel>>(salesCoreModels);
                #endregion

                #region Filling ViewBag
                ViewBag.SaleFilterCustomerFirstNameValue = saleFilterViewModel.CustomerFirstName;
                ViewBag.SaleFilterCustomerLastNameValue = saleFilterViewModel.CustomerLastName;
                ViewBag.SaleFilterDateFromValue = saleFilterViewModel.DateFrom;
                ViewBag.SaleFilterDateToValue = saleFilterViewModel.DateTo;
                ViewBag.SaleFilterManagerLastNameValue = saleFilterViewModel.ManagerLastName;
                ViewBag.SaleFilterProductNameValue = saleFilterViewModel.ProductName;
                ViewBag.SaleFilterSumFromValue = saleFilterViewModel.SumFrom;
                ViewBag.SaleFilterSumToValue = saleFilterViewModel.SumTo;
                #endregion

                return PartialView("Partial/_SaleTable", salesViewModels);
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return PartialView("Partial/_SaleTable");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create(SaleViewModel sale)
        {
            try
            {
                #region Validation
                if (!ModelState.IsValid)
                {
                    return View(sale);
                }
                #endregion

                await _saleService.AddAsync(_mapper.Map<SaleCoreModel>(sale)).ConfigureAwait(false);

                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return View();
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var saleCoreModel = await _saleService.GetAsync(id).ConfigureAwait(false);

                var saleViewModel = _mapper.Map<SaleViewModel>(saleCoreModel);

                return View(saleViewModel);
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return View();
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(SaleViewModel sale)
        {
            try
            {
                #region Validation
                if (!ModelState.IsValid)
                {
                    return View(sale);
                }
                #endregion

                await _saleService.UpdateAsync(_mapper.Map<SaleCoreModel>(sale)).ConfigureAwait(false);

                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return View();
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
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