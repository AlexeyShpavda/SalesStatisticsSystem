using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using X.PagedList;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;
using SalesStatisticsSystem.Contracts.Core.Services;
using SalesStatisticsSystem.WebApp.Models.Filters;
using SalesStatisticsSystem.WebApp.Models.SaleViewModels;

namespace SalesStatisticsSystem.WebApp.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;

            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Index(int? page)
        {
            try
            {
                ViewBag.ProductFilter = new ProductFilterModel();

                const int pageSize = 3;

                var productsDto = await _productService.GetUsingPagedListAsync(page ?? 1, pageSize);

                var productsViewModels =
                        _mapper.Map<IPagedList<ProductViewModel>>(productsDto);

                return View(productsViewModels);
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return View();
            }
        }

        [HttpGet]
        public async Task<ActionResult> Find(ProductFilterModel productFilterModel)
        {
            try
            {
                const int pageSize = 3;

                if (!ModelState.IsValid)
                {
                    var dto = await _productService.GetUsingPagedListAsync(productFilterModel.Page ?? 1, pageSize)
                        .ConfigureAwait(false);

                    var viewModels = _mapper.Map<IPagedList<ProductViewModel>>(dto);

                    return PartialView("Partial/_ProductTable", viewModels);
                }

                IPagedList<ProductDto> productsDto;
                if (productFilterModel.Name == null)
                {
                    productsDto = await _productService.GetUsingPagedListAsync(productFilterModel.Page ?? 1, pageSize)
                        .ConfigureAwait(false);
                }
                else
                {
                    productsDto = await _productService.GetUsingPagedListAsync(productFilterModel.Page ?? 1, pageSize,
                            x => x.Name.Contains(productFilterModel.Name))
                        .ConfigureAwait(false);
                }

                var productsViewModels = _mapper.Map<IPagedList<ProductViewModel>>(productsDto);

                ViewBag.ProductFilterNameValue = productFilterModel.Name;

                return PartialView("Partial/_ProductTable", productsViewModels);
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return PartialView("Partial/_ProductTable");
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

        [HttpGet]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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

        [HttpGet]
        [Authorize(Roles = "Admin")]
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
