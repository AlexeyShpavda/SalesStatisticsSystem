using System.Web.Mvc;
using AutoMapper;
using SalesStatisticsSystem.Core.Services;

namespace SalesStatisticsSystem.WebApplication.Controllers
{
    public class ManagerController : Controller
    {
        private readonly ManagerService _managerService;

        private readonly IMapper _mapper;

        public ManagerController()
        {
            _mapper = Support.AutoMapper.CreateConfiguration().CreateMapper();

            _managerService = new ManagerService();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                // TODO: make delete by id

                Delete(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
    }
}
