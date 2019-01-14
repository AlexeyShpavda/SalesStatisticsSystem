using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;
using AutoMapper;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;
using SalesStatisticsSystem.Contracts.Core.Services;
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
                ViewBag.SaleFilter = new SaleFilterModel();

                var salesDto = await _saleService.GetUsingPagedListAsync(page ?? 1, _pageSize);

                var salesViewModels =
                        _mapper.Map<IPagedList<SaleViewModel>>(salesDto);

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
                var salesForGraphDto =
                    await _saleService.GetUsingPagedListAsync(page ?? 1, _numberOfRecordsToCreateSchedule, null,
                        SortDirection.Descending);

                var salesForGraphViewModels =
                    _mapper.Map<IEnumerable<SaleViewModel>>(salesForGraphDto).ToList();

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
        public async Task<ActionResult> Find(SaleFilterModel saleFilterModel)
        {
            try
            {
                #region Validation
                if (!ModelState.IsValid)
                {
                    var dto = await _saleService.GetUsingPagedListAsync(saleFilterModel.Page ?? 1, _pageSize)
                        .ConfigureAwait(false);

                    var viewModels = _mapper.Map<IPagedList<SaleViewModel>>(dto);

                    return PartialView("Partial/_SaleTable", viewModels);
                }
                #endregion

                # region Filter
                // Can be Transferred to Core.

                IPagedList<SaleDto> salesDto;
                if (saleFilterModel.CustomerFirstName == null &&
                    saleFilterModel.CustomerLastName == null &&
                    saleFilterModel.DateFrom == null &&
                    saleFilterModel.DateTo == null &&
                    saleFilterModel.ManagerLastName == null &&
                    saleFilterModel.ProductName == null &&
                    saleFilterModel.SumFrom == null &&
                    saleFilterModel.SumTo == null)
                {
                    salesDto = await _saleService.GetUsingPagedListAsync(saleFilterModel.Page ?? 1, _pageSize)
                        .ConfigureAwait(false);
                }
                else
                {
                    salesDto = await _saleService.GetUsingPagedListAsync(
                        saleFilterModel.Page ?? 1, _pageSize, x =>
                            (x.Date >= saleFilterModel.DateFrom && x.Date <= saleFilterModel.DateTo) &&
                            (x.Sum >= saleFilterModel.SumFrom && x.Sum <= saleFilterModel.SumTo) &&
                            x.Customer.FirstName.Contains(saleFilterModel.CustomerFirstName) &&
                            x.Customer.LastName.Contains(saleFilterModel.CustomerLastName) &&
                            x.Manager.LastName.Contains(saleFilterModel.ManagerLastName) &&
                            x.Product.Name.Contains(saleFilterModel.ProductName)).ConfigureAwait(false);
                }

                var salesViewModels = _mapper.Map<IPagedList<SaleViewModel>>(salesDto);
                #endregion

                #region Filling ViewBag
                ViewBag.SaleFilterCustomerFirstNameValue = saleFilterModel.CustomerFirstName;
                ViewBag.SaleFilterCustomerLastNameValue = saleFilterModel.CustomerLastName;
                ViewBag.SaleFilterDateFromValue = saleFilterModel.DateFrom;
                ViewBag.SaleFilterDateToValue = saleFilterModel.DateTo;
                ViewBag.SaleFilterManagerLastNameValue = saleFilterModel.ManagerLastName;
                ViewBag.SaleFilterProductNameValue = saleFilterModel.ProductName;
                ViewBag.SaleFilterSumFromValue = saleFilterModel.SumFrom;
                ViewBag.SaleFilterSumToValue = saleFilterModel.SumTo;
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

                await _saleService.AddAsync(_mapper.Map<SaleDto>(sale)).ConfigureAwait(false);

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

                await _saleService.UpdateAsync(_mapper.Map<SaleDto>(sale)).ConfigureAwait(false);

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