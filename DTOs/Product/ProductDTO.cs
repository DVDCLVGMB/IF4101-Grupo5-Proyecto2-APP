namespace Steady_Management_App.DTOs
{
    public class ProductDTO
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public int CategoryId { get; set; } = 0;

        public float Price { get; set; } = 0f;
    }
}
