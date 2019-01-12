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
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        private readonly IMapper _mapper;

        public CustomerController()
        {
            _mapper = Support.Adapter.AutoMapper.CreateConfiguration().CreateMapper();

            _customerService = new CustomerService();
        }

        public async Task<ActionResult> Index()
        {
            try
            {
                ViewBag.CustomerFilter = new CustomerFilter();

                var customersDto = await _customerService.GetAllAsync().ConfigureAwait(false);

                var customersViewModels = _mapper.Map<IEnumerable<CustomerViewModel>>(customersDto);

                return View(customersViewModels);
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return View();
            }
        }

        public async Task<ActionResult> Find(CustomerFilter customerFilter)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var dto = await _customerService.GetAllAsync().ConfigureAwait(false);

                    var viewModels = _mapper.Map<IEnumerable<CustomerViewModel>>(dto);

                    return PartialView("Partial/_CustomerTable", viewModels);
                }

                IEnumerable<CustomerDto> customersDto;
                if (customerFilter.FirstName == null && customerFilter.LastName == null)
                {
                    customersDto = await _customerService.GetAllAsync().ConfigureAwait(false);
                }
                else
                {
                    customersDto = await _customerService.FindAsync(x =>
                            x.FirstName.Contains(customerFilter.FirstName) || x.LastName.Contains(customerFilter.LastName))
                        .ConfigureAwait(false);
                }

                var customersViewModels = _mapper.Map<IEnumerable<CustomerViewModel>>(customersDto);

                return PartialView("Partial/_CustomerTable", customersViewModels);
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return PartialView("Partial/_CustomerTable");
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CustomerViewModel customer)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(customer);
                }

                await _customerService.AddAsync(_mapper.Map<CustomerDto>(customer)).ConfigureAwait(false);

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
                var customerDto = await _customerService.GetAsync(id).ConfigureAwait(false);

                var customerViewModel = _mapper.Map<CustomerViewModel>(customerDto);

                return View(customerViewModel);
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return View();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(CustomerViewModel customer)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(customer);
                }

                await _customerService.UpdateAsync(_mapper.Map<CustomerDto>(customer)).ConfigureAwait(false);

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
