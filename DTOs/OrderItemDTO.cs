using System.ComponentModel;

namespace Steady_Management_App.DTOs
{
    public class OrderItemDTO : INotifyPropertyChanged
    {
        private int _quantity;

        public ProductDTO Product { get; set; } = null!;

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

        public decimal Subtotal => Product.Price * Quantity;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
