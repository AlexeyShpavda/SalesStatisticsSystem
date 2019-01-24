using System;
using System.Configuration;
using System.Threading.Tasks;
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
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        private readonly IMapper _mapper;

        private readonly int _pageSize;

        public CustomerController(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;

            _mapper = mapper;

            _pageSize = int.Parse(ConfigurationManager.AppSettings["numberOfRecordsPerPage"]);
        }

        [HttpGet]
        public async Task<ActionResult> Index(int? page)
        {
            try
            {
                ViewBag.CustomerFilter = new CustomerFilterViewModel();

                var customersCoreModels = await _customerService.GetUsingPagedListAsync(page ?? 1, _pageSize);

                var customersViewModels =
                        _mapper.Map<IPagedList<CustomerViewModel>>(customersCoreModels);

                return View(customersViewModels);
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return View();
            }
        }

        [HttpGet]
        public async Task<ActionResult> Find(CustomerFilterViewModel customerFilterViewModel)
        {
            try
            {
                #region Validation
                if (!ModelState.IsValid)
                {
                    var coreModels = await _customerService
                        .GetUsingPagedListAsync(customerFilterViewModel.Page ?? 1, _pageSize)
                        .ConfigureAwait(false);

                    var viewModels = _mapper.Map<IPagedList<CustomerViewModel>>(coreModels);

                    return PartialView("Partial/_CustomerTable", viewModels);
                }
                #endregion

                #region Filter
                var customersCoreModels = _customerService.Filter(
                    _mapper.Map<CustomerFilterCoreModel>(customerFilterViewModel), _pageSize);

                var customersViewModels = _mapper.Map<IPagedList<CustomerViewModel>>(customersCoreModels);
                #endregion

                #region Filling ViewBag
                ViewBag.CustomerFilterFirstNameValue = customerFilterViewModel.FirstName;
                ViewBag.CustomerFilterLastNameValue = customerFilterViewModel.LastName;
                #endregion

                return PartialView("Partial/_CustomerTable", customersViewModels);
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return PartialView("Partial/_CustomerTable");
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
        public async Task<ActionResult> Create(CustomerViewModel customerViewModel)
        {
            try
            {
                #region Validation
                if (!ModelState.IsValid)
                {
                    return View(customerViewModel);
                }
                #endregion

                await _customerService.AddAsync(_mapper.Map<CustomerCoreModel>(customerViewModel))
                    .ConfigureAwait(false);

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
                var customerCoreModel = await _customerService.GetAsync(id).ConfigureAwait(false);

                var customerViewModel = _mapper.Map<CustomerViewModel>(customerCoreModel);

                return View(customerViewModel);
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return View();
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(CustomerViewModel customerViewModel)
        {
            try
            {
                #region Validation
                if (!ModelState.IsValid)
                {
                    return View(customerViewModel);
                }
                #endregion

                await _customerService.UpdateAsync(_mapper.Map<CustomerCoreModel>(customerViewModel))
                    .ConfigureAwait(false);

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
                await _customerService.DeleteAsync(id).ConfigureAwait(false);

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
