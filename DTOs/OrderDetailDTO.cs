using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steady_Management_App.DTOs
{
    public class OrderDetailDTO
    {
        private int _quantity;
        public int ProductId { get; set; } 
        public string ProductName { get; set; } = string.Empty;
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                    OnPropertyChanged(nameof(Subtotal)); // recalcula el subtotal dinámicamente
                }
            }
        }
        public decimal UnitPrice { get; set; }

        public decimal Subtotal => Quantity * UnitPrice;

        public bool IsTaxable { get; set; } // Si lo necesitás para cálculo
        public decimal TaxAmount => IsTaxable ? Subtotal * 0.13m : 0;


        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
