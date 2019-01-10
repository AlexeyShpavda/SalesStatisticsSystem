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
            var customersDto = await _customerService.GetAllAsync();

            var customersViewModels = _mapper.Map<IEnumerable<CustomerViewModel>>(customersDto);

            return View(customersViewModels);
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

                await _customerService.AddAsync(_mapper.Map<CustomerDto>(customer));

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
            var customerDto = await _customerService.GetAsync(id);

            var customerViewModel = _mapper.Map<CustomerViewModel>(customerDto);

            return View(customerViewModel);
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

                await _customerService.UpdateAsync(_mapper.Map<CustomerDto>(customer));

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
                await _customerService.DeleteAsync(id);

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
