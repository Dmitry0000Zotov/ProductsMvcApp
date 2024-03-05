using Microsoft.AspNetCore.Mvc;
using ProductsMvcApp.Application.Services.Interfaces;
using ProductsMvcApp.Domain.Entities;
using ProductsMvcApp.Domain.ViewModels;
using ProductsMvcApp.Models;
using System.Net;

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

            if (product.Id != Guid.Empty)
            {
                var productVm = new UpdateProductViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description ?? ""
                };
                var response = await _productService.UpdateProduct(productVm);
                if(response.StatusCode == HttpStatusCode.OK)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var error = new ErrorViewModel
                    {
                        Description = response.Description,
                        StatusCode = response.StatusCode
                    };
                    return RedirectToAction("Error", error);
                }
            }
            else
            {
                var response = await _productService.CreateProduct(product);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var error = new ErrorViewModel
                    {
                        Description = response.Description,
                        StatusCode = response.StatusCode
                    };
                    return RedirectToAction("Error", error);
                }
            }
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

        [HttpGet]
        public async Task<IActionResult> Error(string Description, HttpStatusCode StatusCode)
        {
            var errorVm = new ErrorViewModel()
            {
                Description = Description,
                StatusCode = StatusCode
            };
            return View(errorVm);
        }
    }
}