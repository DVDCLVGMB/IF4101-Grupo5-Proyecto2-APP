using System.ComponentModel.DataAnnotations;
using Steady_Management_App.DTOs;
using Steady_Management_App.Models;

public class OrderDTO
{
    public Client Client { get; set; } = null!;
    public List<OrderItemDTO> Items { get; set; } = new();

    public decimal Total => Items.Sum(i => i.Subtotal);

    public decimal TotalTaxes => Items
     .Where(i => i.Product.IsTaxable) // ← usamos la propiedad que ya tenés
     .Sum(i => i.Subtotal * 0.13m);   // ← 13% de IVA

    public int ClientId { get; set; }

    public int EmployeeId { get; set; }

    public int CityId { get; set; }

    public DateTime OrderDate { get; set; }

    //public List<OrderDetailDTO> OrderDetails { get; set; } = new();

    public int PaymentMethodId { get; set; }

    public string PaymentMethodName { get; set; } = string.Empty;

    public DateTime PaymentDate { get; set; }

    public string CreditCardNumber { get; set; } = string.Empty;


    public decimal PaymentQuantity { get; set; }

}

