namespace Steady_Management_App.DTOs
{
    public class OrderItemDTO
    {
        public ProductDTO Product { get; set; } = null!;
        public int Quantity { get; set; } = 1;

        public decimal Subtotal => Product.Price * Quantity;
        public decimal TaxAmount => Product.IsTaxable ? Subtotal * 0.13m : 0m;
    }
}
