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

        public ManagerController(IManagerService managerService, IMapper mapper)
        {
            _managerService = managerService;

            _mapper = mapper;
        }

        public async Task<ActionResult> Index()
        {
            try
            {
                ViewBag.ManagerFilter = new ManagerFilter();

                var managersDto = await _managerService.GetAllAsync().ConfigureAwait(false);

                var managersViewModels = _mapper.Map<IEnumerable<ManagerViewModel>>(managersDto);

                return View(managersViewModels);
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return View();
            }
        }

        public async Task<ActionResult> Find(ManagerFilter managerFilter)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var dto = await _managerService.GetAllAsync().ConfigureAwait(false);

                    var viewModels = _mapper.Map<IEnumerable<ManagerViewModel>>(dto);

                    return PartialView("Partial/_ManagerTable", viewModels);
                }

                IEnumerable<ManagerDto> managersDto;
                if (managerFilter.LastName == null)
                {
                    managersDto = await _managerService.GetAllAsync().ConfigureAwait(false);
                }
                else
                {
                    managersDto = await _managerService.FindAsync(x =>
                        x.LastName.Contains(managerFilter.LastName)).ConfigureAwait(false);
                }

                var managersViewModels = _mapper.Map<IEnumerable<ManagerViewModel>>(managersDto);

                return PartialView("Partial/_ManagerTable", managersViewModels);
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return PartialView("Partial/_ManagerTable");
            }
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

                await _managerService.AddAsync(_mapper.Map<ManagerDto>(manager)).ConfigureAwait(false);

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
                var managerDto = await _managerService.GetAsync(id).ConfigureAwait(false);

                var managerViewModel = _mapper.Map<ManagerViewModel>(managerDto);

                return View(managerViewModel);
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return View();
            }
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

                await _managerService.UpdateAsync(_mapper.Map<ManagerDto>(manager)).ConfigureAwait(false);

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
                await _managerService.DeleteAsync(id).ConfigureAwait(false);

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
