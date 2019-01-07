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
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        private readonly IMapper _mapper;

        public ProductController()
        {
            _mapper = Support.AutoMapper.CreateConfiguration().CreateMapper();

            _productService = new ProductService();
        }

        public async Task<ActionResult> Index()
        {
            var productsDto = await _productService.GetAllAsync();

            var productsViewModels = _mapper.Map<IEnumerable<ProductViewModel>>(productsDto);

            return View(productsViewModels);
        }

        public ActionResult Details(int id)
        {
            // TODO: Make additional fields (description)

            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ProductViewModel product)
        {
            try
            {
                _productService.Add(_mapper.Map<ProductDto>(product));

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
            var productDto = _productService.GetAsync(id);

            var productViewModel = _mapper.Map<ProductViewModel>(productDto);

            return View(productViewModel);
        }

        [HttpPost]
        public ActionResult Edit(ProductViewModel product)
        {
            try
            {
                _productService.Update(_mapper.Map<ProductDto>(product));

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
                // TODO: make delete by id

                //var productDto = _productService.GetProductAsync(id);

                //var productViewModel = _mapper.Map<ProductViewModel>(productDto);

                //_productService.Delete(_mapper.Map<ProductDto>(productViewModel));

                _productService.Delete(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
    }
}
