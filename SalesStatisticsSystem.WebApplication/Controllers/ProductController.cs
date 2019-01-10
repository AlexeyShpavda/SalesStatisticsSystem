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
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        private readonly IMapper _mapper;

        public ProductController()
        {
            _mapper = Support.Adapter.AutoMapper.CreateConfiguration().CreateMapper();

            _productService = new ProductService();
        }

        public async Task<ActionResult> Index()
        {
            var productsDto = await _productService.GetAllAsync();

            var productsViewModels = _mapper.Map<IEnumerable<ProductViewModel>>(productsDto);

            return View(productsViewModels);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(ProductViewModel product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(product);
                }

                await _productService.AddAsync(_mapper.Map<ProductDto>(product));

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
            var productDto = await _productService.GetAsync(id);

            var productViewModel = _mapper.Map<ProductViewModel>(productDto);

            return View(productViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(ProductViewModel product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(product);
                }

                await _productService.UpdateAsync(_mapper.Map<ProductDto>(product));

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
                await _productService.DeleteAsync(id);

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
