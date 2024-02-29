using Newtonsoft.Json;
using ProductsMvcApp.Application.Helpers;
using ProductsMvcApp.Application.Services.Interfaces;
using ProductsMvcApp.Domain.Entities;
using ProductsMvcApp.Domain.ViewModels;
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

        public async Task<Guid> CreateProduct(Product product)
        {
            var content = new MultipartFormDataContent
            {
                { new StringContent(JsonConvert.SerializeObject(product.Name), Encoding.UTF8, MediaTypeNames.Application.Json), "Name" },
                { new StringContent(JsonConvert.SerializeObject(product.Description ?? ""), Encoding.UTF8, MediaTypeNames.Application.Json), "Description" }
            };

            var response = await _client.PostAsync(actionApiPath + "CreateProduct", content);

            return await response.ReadContentAsync<Guid>();
        }

        public async Task<Product> UpdateProduct(UpdateProductViewModel product)
        {
            var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, MediaTypeNames.Application.Json);

            var response = await _client.PutAsync(actionApiPath + "UpdateProduct", content);

            return await response.ReadContentAsync<Product>();
        }

        public async Task<Guid> DeleteProduct(Guid productId)
        {
            var response = await _client.DeleteAsync(actionApiPath + "DeleteProduct?Id=" + productId);

            return await response.ReadContentAsync<Guid>();
        }
    }
}
