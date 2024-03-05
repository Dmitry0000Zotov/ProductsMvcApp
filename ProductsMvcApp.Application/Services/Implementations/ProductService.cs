using Newtonsoft.Json;
using ProductsMvcApp.Application.Helpers;
using ProductsMvcApp.Application.Services.Interfaces;
using ProductsMvcApp.Domain.Entities;
using ProductsMvcApp.Domain.Response;
using ProductsMvcApp.Domain.ViewModels;
using System.Net;
using System.Net.Mime;
using System.Text;

namespace ProductsMvcApp.Application.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _client;
        public const string actionApiPath = "https://localhost:7291/products/product/";

        public ProductService(HttpClient client) => _client = client;

        public async Task<IEnumerable<Product>> GetProductsList(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var response = await _client.GetAsync(actionApiPath + "getProductList?name=" + name);
                return await response.ReadContentAsync<List<Product>>();
            }
            else
            {
                var response = await _client.GetAsync(actionApiPath + "getProductList");
                return await response.ReadContentAsync<List<Product>>();
            }
        }

        public async Task<Product> GetProduct(Guid productId)
        {
            var response = await _client.GetAsync(actionApiPath + "GetProduct?Id=" + productId);

            return await response.ReadContentAsync<Product>();
        }

        public async Task<BaseResponse<Guid>> CreateProduct(Product product)
        {
            var content = new MultipartFormDataContent
            {
                { new StringContent(JsonConvert.SerializeObject(product.Name), Encoding.UTF8, MediaTypeNames.Application.Json), "Name" },
                { new StringContent(JsonConvert.SerializeObject(product.Description ?? ""), Encoding.UTF8, MediaTypeNames.Application.Json), "Description" }
            };
            try
            {
                var response = await _client.PostAsync(actionApiPath + "CreateProduct", content);
                var productId = await response.ReadContentAsync<Guid>();

                return new BaseResponse<Guid>()
                {
                    Data = productId,
                    StatusCode = response.StatusCode,
                    Description = $"Function-CreateProduct: End with status code {response.StatusCode}. Message: {response.RequestMessage}"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Guid>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Description = $"Function-CreateProduct: End with status code {HttpStatusCode.InternalServerError}. Message: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponse<Product>> UpdateProduct(UpdateProductViewModel product)
        {
            var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, MediaTypeNames.Application.Json);
            try
            {
                var response = await _client.PutAsync(actionApiPath + "UpdateProduct", content);
                var updatedProduct = await response.ReadContentAsync<Product>();

                return new BaseResponse<Product>()
                {
                    Data = updatedProduct,
                    StatusCode = response.StatusCode,
                    Description = $"Function-UpdateProduct: End with status code {response.StatusCode}. Message: {response.RequestMessage}"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Product>()
                {
                    StatusCode= HttpStatusCode.InternalServerError,
                    Description = $"Function-UpdateProduct: End with status code {HttpStatusCode.InternalServerError}. Message: {ex.Message}"
                };
            }
        }

        public async Task<Guid> DeleteProduct(Guid productId)
        {
            var response = await _client.DeleteAsync(actionApiPath + "DeleteProduct?Id=" + productId);

            return await response.ReadContentAsync<Guid>();
        }
    }
}
