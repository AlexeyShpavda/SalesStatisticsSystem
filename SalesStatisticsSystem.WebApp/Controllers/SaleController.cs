using System;
using System.Collections.Generic;
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
    public class SaleController : Controller
    {
        private readonly ISaleService _saleService;

        private readonly IMapper _mapper;

        public SaleController(ISaleService saleService, IMapper mapper)
        {
            _saleService = saleService;

            _mapper = mapper;
        }

        public async Task<ActionResult> Index(int? page)
        {
            try
            {
                ViewBag.SaleFilter = new SaleFilterModel();

                const int pageSize = 3;

                var salesDto = await _saleService.GetUsingPagedListAsync(page ?? 1, pageSize);
                var salesForGraphDto =
                    await _saleService.GetUsingPagedListAsync(page ?? 1, 100, null, SortDirection.Descending);


                var salesViewModels =
                        _mapper.Map<IPagedList<SaleViewModel>>(salesDto);
                var salesForGraphViewModels =
                    _mapper.Map<IEnumerable<SaleViewModel>>(salesForGraphDto).ToList();

                #region Chart

                var uniqueProducts = salesForGraphViewModels.Select(x => x.Product.Name).Distinct();

                var productSalesQuantity = uniqueProducts
                    .Select(product => salesForGraphViewModels.Count(x => x.Product.Name == product));

                ViewBag.UniqueProducts = uniqueProducts;
                ViewBag.ProductSalesQuantity = productSalesQuantity;
                #endregion

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
                const int pageSize = 3;

                if (!ModelState.IsValid)
                {
                    var dto = await _saleService.GetUsingPagedListAsync(saleFilterModel.Page ?? 1, pageSize)
                        .ConfigureAwait(false);

                    var viewModels = _mapper.Map<IPagedList<SaleViewModel>>(dto);

                    return PartialView("Partial/_SaleTable", viewModels);
                }

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
                    salesDto = await _saleService.GetUsingPagedListAsync(saleFilterModel.Page ?? 1, pageSize)
                        .ConfigureAwait(false);
                }
                else
                {
                    salesDto = await _saleService.GetUsingPagedListAsync(
                        saleFilterModel.Page ?? 1, pageSize, x =>
                            (x.Date >= saleFilterModel.DateFrom && x.Date <= saleFilterModel.DateTo) &&
                            (x.Sum >= saleFilterModel.SumFrom && x.Sum <= saleFilterModel.SumTo) &&
                            x.Customer.FirstName.Contains(saleFilterModel.CustomerFirstName) &&
                            x.Customer.LastName.Contains(saleFilterModel.CustomerLastName) &&
                            x.Manager.LastName.Contains(saleFilterModel.ManagerLastName) &&
                            x.Product.Name.Contains(saleFilterModel.ProductName)).ConfigureAwait(false);
                }

                var salesViewModels = _mapper.Map<IPagedList<SaleViewModel>>(salesDto);

                ViewBag.SaleFilterCustomerFirstNameValue = saleFilterModel.CustomerFirstName;
                ViewBag.SaleFilterCustomerLastNameValue = saleFilterModel.CustomerLastName;
                ViewBag.SaleFilterDateFromValue = saleFilterModel.DateFrom;
                ViewBag.SaleFilterDateToValue = saleFilterModel.DateTo;
                ViewBag.SaleFilterManagerLastNameValue = saleFilterModel.ManagerLastName;
                ViewBag.SaleFilterProductNameValue = saleFilterModel.ProductName;
                ViewBag.SaleFilterSumFromValue = saleFilterModel.SumFrom;
                ViewBag.SaleFilterSumToValue = saleFilterModel.SumTo;

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