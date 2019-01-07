using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;
using SalesStatisticsSystem.Contracts.Core.Services;
using SalesStatisticsSystem.Core.Services;
using SalesStatisticsSystem.WebApplication.Models;

namespace SalesStatisticsSystem.WebApplication.Controllers
{
    public class ManagerController : Controller
    {
        private readonly IManagerService _managerService;

        private readonly IMapper _mapper;

        public ManagerController()
        {
            _mapper = Support.AutoMapper.CreateConfiguration().CreateMapper();

            _managerService = new ManagerService();
        }

        public async Task<ActionResult> Index()
        {
            var managersDto = await _managerService.GetAllAsync();

            var managersViewModels = _mapper.Map<IEnumerable<ManagerViewModel>>(managersDto);

            return View(managersViewModels);
        }

        public ActionResult Details(int id)
        {
            // TODO: Make additional fields (Email, Phone number)

            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ManagerViewModel manager)
        {
            try
            {
                _managerService.Add(_mapper.Map<ManagerDto>(manager));

                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            var managerDto = _managerService.GetAsync(id);

            var managerViewModel = _mapper.Map<ManagerViewModel>(managerDto);

            return View(managerViewModel);
        }

        [HttpPost]
        public ActionResult Edit(ManagerViewModel manager)
        {
            try
            {
                _managerService.Update(_mapper.Map<ManagerDto>(manager));

                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                _managerService.Delete(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
    }
}
