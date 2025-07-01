using Steady_Management_App.Models;

namespace Steady_Management_App.DTOs
{
    public class ProductDTO
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public int CategoryId { get; set; } = 0;

        public decimal Price { get; set; } = 0;

        public bool IsTaxable { get; set; }

        public static implicit operator ProductDTO(Steady_Management_App.Models.Product p)
        {
            if (p == null) return null!;  // o lanza ArgumentNullException

            return new ProductDTO
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                CategoryId = p.CategoryId,
                Price = p.Price,
                IsTaxable = p.IsTaxable
            };
        }
    }
}
