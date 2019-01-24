using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using SalesStatisticsSystem.Core.Contracts.Models.Filters;
using SalesStatisticsSystem.Core.Contracts.Models.Sales;
using SalesStatisticsSystem.Core.Contracts.Services;
using X.PagedList;
using SalesStatisticsSystem.WebApp.Models.Filters;
using SalesStatisticsSystem.WebApp.Models.SaleViewModels;

namespace SalesStatisticsSystem.WebApp.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        private readonly IMapper _mapper;

        private readonly int _pageSize;

        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;

            _mapper = mapper;

            _pageSize = int.Parse(ConfigurationManager.AppSettings["numberOfRecordsPerPage"]);
        }

        [HttpGet]
        public async Task<ActionResult> Index(int? page)
        {
            try
            {
                ViewBag.ProductFilter = new ProductFilterViewModel();

                var productsCoreModels = await _productService.GetUsingPagedListAsync(page ?? 1, _pageSize);

                var productsViewModels =
                        _mapper.Map<IPagedList<ProductViewModel>>(productsCoreModels);

                return View(productsViewModels);
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return View();
            }
        }

        [HttpGet]
        public async Task<ActionResult> Find(ProductFilterViewModel productFilterViewModel)
        {
            try
            {
                #region Validation
                if (!ModelState.IsValid)
                {
                    var coreModels = await _productService
                        .GetUsingPagedListAsync(productFilterViewModel.Page ?? 1, _pageSize)
                        .ConfigureAwait(false);

                    var viewModels = _mapper.Map<IPagedList<ProductViewModel>>(coreModels);

                    return PartialView("Partial/_ProductTable", viewModels);
                }
                #endregion

                #region Filter
                var productsCoreModels = _productService.Filter(
                    _mapper.Map<ProductFilterCoreModel>(productFilterViewModel), _pageSize);

                var productsViewModels = _mapper.Map<IPagedList<ProductViewModel>>(productsCoreModels);
                #endregion

                #region Filling ViewBag
                ViewBag.ProductFilterNameValue = productFilterViewModel.Name;
                #endregion

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
                #region Validation
                if (!ModelState.IsValid)
                {
                    return View(product);
                }
                #endregion

                await _productService.AddAsync(_mapper.Map<ProductCoreModel>(product)).ConfigureAwait(false);

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
                var productCoreModel = await _productService.GetAsync(id).ConfigureAwait(false);

                var productViewModel = _mapper.Map<ProductViewModel>(productCoreModel);

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
                #region Validation
                if (!ModelState.IsValid)
                {
                    return View(product);
                }
                #endregion

                await _productService.UpdateAsync(_mapper.Map<ProductCoreModel>(product)).ConfigureAwait(false);

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
