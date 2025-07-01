using System.Threading.Tasks;
using System.Collections.Generic;
using Steady_Management_App.DTOs;
using Steady_Management_App.Models;

namespace Steady_Management_App.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<Product?> AddProductAsync(string name, int categoryId, decimal price, bool isTaxable);
        Task<bool> UpdateProductAsync(Product p);
        Task<bool> DeleteProductAsync(int id);
    }
}