using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;
using SalesStatisticsSystem.Contracts.Core.Services;
using SalesStatisticsSystem.WebApp.Models.Filters;
using SalesStatisticsSystem.WebApp.Models.SaleViewModels;

namespace SalesStatisticsSystem.WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;

            _mapper = mapper;
        }

        public async Task<ActionResult> Index()
        {
            try
            {
                ViewBag.ProductFilter = new ProductFilterModel();

                var productsDto = await _productService.GetAllAsync().ConfigureAwait(false);

                var productsViewModels = _mapper.Map<IEnumerable<ProductViewModel>>(productsDto);

                return View(productsViewModels);
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return View();
            }
        }

        public async Task<ActionResult> Find(ProductFilterModel productFilterModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var dto = await _productService.GetAllAsync().ConfigureAwait(false);

                    var viewModels = _mapper.Map<IEnumerable<ProductViewModel>>(dto);

                    return PartialView("Partial/_ProductTable", viewModels);
                }

                IEnumerable<ProductDto> productsDto;
                if (productFilterModel.Name == null)
                {
                    productsDto = await _productService.GetAllAsync().ConfigureAwait(false);
                }
                else
                {
                    productsDto = await _productService.FindAsync(x => x.Name.Contains(productFilterModel.Name))
                        .ConfigureAwait(false);
                }

                var productsViewModels = _mapper.Map<IEnumerable<ProductViewModel>>(productsDto);

                return PartialView("Partial/_ProductTable", productsViewModels);
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return PartialView("Partial/_ProductTable");
            }
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

                await _productService.AddAsync(_mapper.Map<ProductDto>(product)).ConfigureAwait(false);

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
                var productDto = await _productService.GetAsync(id).ConfigureAwait(false);

                var productViewModel = _mapper.Map<ProductViewModel>(productDto);

                return View(productViewModel);
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return View();
            }
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

                await _productService.UpdateAsync(_mapper.Map<ProductDto>(product)).ConfigureAwait(false);

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
                await _productService.DeleteAsync(id).ConfigureAwait(false);

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
