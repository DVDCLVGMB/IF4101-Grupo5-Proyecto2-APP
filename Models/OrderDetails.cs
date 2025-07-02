using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Steady_Management_App.Models
{
    public class OrderDetail : INotifyPropertyChanged
    {
        private int _orderId;
        public int OrderId
        {
            get => _orderId;
            set { _orderId = value; OnPropertyChanged(); }
        }

        private int _productId;
        public int ProductId
        {
            get => _productId;
            set { _productId = value; OnPropertyChanged(); }
        }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set { _quantity = value; OnPropertyChanged(); }
        }

        private float _unitPrice;
        public float UnitPrice
        {
            get => _unitPrice;
            set { _unitPrice = value; OnPropertyChanged(); }
        }

        public OrderDetail() { }

        public OrderDetail(int orderId, int productId, int quantity, float unitPrice)
        {
            _orderId = orderId;
            _productId = productId;
            _quantity = quantity;
            _unitPrice = unitPrice;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}