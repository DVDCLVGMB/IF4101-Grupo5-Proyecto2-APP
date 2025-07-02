using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Steady_Management_App.Models
{
    public class OrderDetail
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public bool IsTaxable { get; set; }

        public decimal Subtotal => UnitPrice * Quantity;

        // Este valor se asigna desde el ViewModel justo al momento de agregar el producto
        public decimal TaxAmount { get; set; }
    }
}