using Steady_Management_App.DTOs;
using Steady_Management_App.Models;

public class OrderDTO
{
    public List<OrderItemDTO> Items { get; set; } = new();
    public Client Customer { get; set; } = null!;

    public decimal Total => Items.Sum(i => i.Subtotal + i.TaxAmount);
    public decimal TotalTaxes => Items.Sum(i => i.TaxAmount);
}
