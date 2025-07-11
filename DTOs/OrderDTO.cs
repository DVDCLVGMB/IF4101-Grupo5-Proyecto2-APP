﻿using Steady_Management_App.DTOs;
using Steady_Management_App.Models;

public class OrderDTO
{
    public Client Client { get; set; } = null!;

    public List<OrderDetailDTO> OrderDetails { get; set; } = new(); // ✅ CAMBIO

    public decimal Total => OrderDetails.Sum(i => i.Subtotal);

    public decimal TotalTaxes => OrderDetails
     .Where(i => i.IsTaxable)
     .Sum(i => i.Subtotal * 0.13m);

    public int ClientId { get; set; }
    public int EmployeeId { get; set; }
    public int CityId { get; set; }
    public DateTime OrderDate { get; set; }

    public int PaymentMethodId { get; set; }
    public string PaymentMethodName { get; set; } = string.Empty;
    public DateTime PaymentDate { get; set; }
    public string CreditCardNumber { get; set; } = string.Empty;
    public decimal PaymentQuantity { get; set; }
    public int OrderId { get; set; }
}
