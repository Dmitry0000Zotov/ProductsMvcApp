using ProductsMvcApp.Domain.Entities;
using ProductsMvcApp.Domain.Response;
using ProductsMvcApp.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductsMvcApp.Application.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsList(string name);
        Task<Product> GetProduct(Guid productId);
        Task<BaseResponse<Guid>> CreateProduct(Product product);
        Task<BaseResponse<Product>> UpdateProduct(UpdateProductViewModel product);
        Task<Guid> DeleteProduct(Guid productId);
    }
}
