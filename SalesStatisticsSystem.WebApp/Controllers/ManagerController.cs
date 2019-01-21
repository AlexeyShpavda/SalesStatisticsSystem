﻿using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using SalesStatisticsSystem.Core.Contracts.Models;
using SalesStatisticsSystem.Core.Contracts.Services;
using SalesStatisticsSystem.WebApp.Models.Filters;
using SalesStatisticsSystem.WebApp.Models.SaleViewModels;
using X.PagedList;

namespace SalesStatisticsSystem.WebApp.Controllers
{
    [Authorize]
    public class ManagerController : Controller
    {
        private readonly IManagerService _managerService;

        private readonly IMapper _mapper;

        private readonly int _pageSize;

        public ManagerController(IManagerService managerService, IMapper mapper)
        {
            _managerService = managerService;

            _mapper = mapper;

            _pageSize = int.Parse(ConfigurationManager.AppSettings["numberOfRecordsPerPage"]);
        }

        [HttpGet]
        public async Task<ActionResult> Index(int? page)
        {
            try
            {
                ViewBag.ManagerFilter = new ManagerFilterViewModel();

                var managersCoreModels = await _managerService.GetUsingPagedListAsync(page ?? 1, _pageSize);

                var managersViewModels =
                        _mapper.Map<IPagedList<ManagerViewModel>>(managersCoreModels);

                return View(managersViewModels);
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return View();
            }
        }

        [HttpGet]
        public async Task<ActionResult> Find(ManagerFilterViewModel managerFilterModel)
        {
            try
            {
                #region Validation
                if (!ModelState.IsValid)
                {
                    var coreModels = await _managerService
                        .GetUsingPagedListAsync(managerFilterModel.Page ?? 1, _pageSize)
                        .ConfigureAwait(false);

                    var viewModels = _mapper.Map<IPagedList<ManagerViewModel>>(coreModels);

                    return PartialView("Partial/_ManagerTable", viewModels);
                }
                #endregion

                #region Filter
                // Can be Transferred to Core.

                IPagedList<ManagerCoreModel> managersCoreModels;
                if (managerFilterModel.LastName == null)
                {
                    managersCoreModels = await _managerService.GetUsingPagedListAsync(managerFilterModel.Page ?? 1, _pageSize)
                        .ConfigureAwait(false);
                }
                else
                {
                    managersCoreModels = await _managerService.GetUsingPagedListAsync(managerFilterModel.Page ?? 1, _pageSize,
                            x => x.LastName.Contains(managerFilterModel.LastName))
                        .ConfigureAwait(false);
                }

                var managersViewModels = _mapper.Map<IPagedList<ManagerViewModel>>(managersCoreModels);
                #endregion

                #region Filling ViewBag
                ViewBag.ManagerFilterLastNameValue = managerFilterModel.LastName;
                #endregion

                return PartialView("Partial/_ManagerTable", managersViewModels);
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return PartialView("Partial/_ManagerTable");
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
        public async Task<ActionResult> Create(ManagerViewModel managerViewModel)
        {
            try
            {
                #region Validation
                if (!ModelState.IsValid)
                {
                    return View(managerViewModel);
                }
                #endregion

                await _managerService.AddAsync(_mapper.Map<ManagerCoreModel>(managerViewModel)).ConfigureAwait(false);

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
                var managerCoreModel = await _managerService.GetAsync(id).ConfigureAwait(false);

                var managerViewModel = _mapper.Map<ManagerViewModel>(managerCoreModel);

                return View(managerViewModel);
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return View();
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(ManagerViewModel manager)
        {
            try
            {
                #region Validation
                if (!ModelState.IsValid)
                {
                    return View(manager);
                }
                #endregion

                await _managerService.UpdateAsync(_mapper.Map<ManagerCoreModel>(manager)).ConfigureAwait(false);

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
