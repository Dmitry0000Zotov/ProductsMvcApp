using Microsoft.AspNetCore.Mvc;
using ProductsMvcApp.Application.Services.Interfaces;
using ProductsMvcApp.Domain.Entities;
using ProductsMvcApp.Domain.ViewModels;

namespace ProductsMvcApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;

        public HomeController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index(string name)
        {
            var products = await _productService.GetProductsList(name);

            ProductListViewModel model = new ProductListViewModel
            {
                Products = products,
                Name = name
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditProduct(Guid id, bool isEdit)
        {
            if (!isEdit)
                return PartialView();

            var product = await _productService.GetProduct(id);

            return PartialView("EditProduct", product);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct([FromForm] Product product)
        {
            if (product.Name != null)
            {
                if (product.Id != Guid.Empty)
                {
                    var productVm = new UpdateProductViewModel
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description ?? ""
                    };
                    var result = await _productService.UpdateProduct(productVm);
                }
                else
                {
                    var productId = await _productService.CreateProduct(product);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var response = await _productService.GetProduct(id);

            if (response != null)
            {
                return PartialView("DeleteProduct", response);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(Product product)
        {
            if(product.Id != Guid.Empty)
            {
                var productId = await _productService.DeleteProduct(product.Id);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}