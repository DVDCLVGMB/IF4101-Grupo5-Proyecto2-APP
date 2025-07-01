public class ProductCreateDto
{
    // no lleva el ProductId porque el SP lo genera
    public int CategoryId { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public bool IsTaxable { get; set; }
}
