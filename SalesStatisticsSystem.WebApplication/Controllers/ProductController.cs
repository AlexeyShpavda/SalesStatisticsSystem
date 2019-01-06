using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;
using SalesStatisticsSystem.Core.Services;
using SalesStatisticsSystem.WebApplication.Models;

namespace SalesStatisticsSystem.WebApplication.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        private readonly IMapper _mapper;

        public ProductController()
        {
            _mapper = Support.AutoMapper.CreateConfiguration().CreateMapper();

            _productService = new ProductService();
        }

        public async Task<ActionResult> Index()
        {
            var productsDto = await _productService.GetProductsAsync();

            var productsViewModels = _mapper.Map<IEnumerable<ProductViewModel>>(productsDto);

            return View(productsViewModels);
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
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
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            var productDto = _productService.GetProductAsync(id);

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
            catch
            {
                return View();
            }
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Product/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
