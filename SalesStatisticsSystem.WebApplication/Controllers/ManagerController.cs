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
    public class ManagerController : Controller
    {
        private readonly IManagerService _managerService;

        private readonly IMapper _mapper;

        public ManagerController()
        {
            _mapper = Support.Adapter.AutoMapper.CreateConfiguration().CreateMapper();

            _managerService = new ManagerService();
        }

        public async Task<ActionResult> Index()
        {
            ViewBag.ManagerFilter = new ManagerFilter();

            var managersDto = await _managerService.GetAllAsync();

            var managersViewModels = _mapper.Map<IEnumerable<ManagerViewModel>>(managersDto);

            return View(managersViewModels);
        }

        public async Task<ActionResult> Find(ManagerFilter managerFilter)
        {
            IEnumerable<ManagerDto> managersDto;
            if (managerFilter.LastName == null)
            {
                managersDto = await _managerService.GetAllAsync();
            }
            else
            {
                managersDto = await _managerService.FindAsync(x =>
                    x.LastName.Contains(managerFilter.LastName));

            }

            var managersViewModels = _mapper.Map<IEnumerable<ManagerViewModel>>(managersDto);

            return PartialView("Partial/_ManagerTable", managersViewModels);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(ManagerViewModel manager)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(manager);
                }

                await _managerService.AddAsync(_mapper.Map<ManagerDto>(manager));

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
            var managerDto = await _managerService.GetAsync(id);

            var managerViewModel = _mapper.Map<ManagerViewModel>(managerDto);

            return View(managerViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(ManagerViewModel manager)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(manager);
                }

                await _managerService.UpdateAsync(_mapper.Map<ManagerDto>(manager));

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
                await _managerService.DeleteAsync(id);

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
