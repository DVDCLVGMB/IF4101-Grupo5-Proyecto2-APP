using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Steady_Management_App.DTOs;
using Steady_Management_App.Models;
using Steady_Management_App.Services;

namespace Steady_Management_App.ViewModels
{
    public partial class CreateOrderViewModel : ObservableObject
    {
        private readonly OrderApiService _orderApiService;

        [ObservableProperty] private int clientId;
        [ObservableProperty] private int cityId;
        [ObservableProperty] private DateTime orderDate = DateTime.Now;
        [ObservableProperty] private int paymentMethodId;
        [ObservableProperty] private string? creditCardNumber;

        [ObservableProperty] private ObservableCollection<OrderDetailDTO> orderDetails = new();
        [ObservableProperty] private decimal subtotal;
        [ObservableProperty] private decimal taxes;
        [ObservableProperty] private decimal total;

        public CreateOrderViewModel(OrderApiService orderApiService)
        {
            _orderApiService = orderApiService;
            OrderDetails.CollectionChanged += (s, e) =>
            {
                if (e.NewItems != null)
                {
                    foreach (OrderDetailDTO item in e.NewItems)
                    {
                        item.PropertyChanged += Item_PropertyChanged;
                    }
                }

                if (e.OldItems != null)
                {
                    foreach (OrderDetailDTO item in e.OldItems)
                    {
                        item.PropertyChanged -= Item_PropertyChanged;
                    }
                }

                RecalculateTotals();
            };

        }

        private void Item_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(OrderDetailDTO.Quantity))
            {
                RecalculateTotals();
            }
        }


        [RelayCommand]
        public void AddProduct(OrderDetailDTO detail)
        {
            OrderDetails.Add(detail);
            RecalculateTotals();
        }

        [RelayCommand]
        public void RemoveProduct(OrderDetailDTO detail)
        {
            OrderDetails.Remove(detail);
            RecalculateTotals();
        }

        public void RecalculateTotals()
        {
            Subtotal = OrderDetails.Sum(d => d.Subtotal);
            Taxes = OrderDetails.Sum(d => d.TaxAmount);
            Total = Subtotal + Taxes;
        }

        [RelayCommand]
        public async Task FinalizarPedidoAsync()
        {
            if (paymentMethodId == 2 && (string.IsNullOrWhiteSpace(creditCardNumber) || creditCardNumber.Length < 12))
            {
                MessageBox.Show("Debe ingresar un número de tarjeta válido.");
                return;
            }

            if (OrderDetails.Count == 0)
            {
                MessageBox.Show("Debe agregar al menos un producto.");
                return;
            }

            var orderDto = new OrderDTO
            {
                ClientId = ClientId,
                CityId = CityId,
                EmployeeId = 4,
                OrderDate = OrderDate,
                PaymentMethodId = PaymentMethodId,
                CreditCardNumber = PaymentMethodId == 2 ? CreditCardNumber : null,
                PaymentDate = OrderDate,
                OrderDetails = OrderDetails.Select(od => new OrderDetailDTO
                {
                    ProductId = od.ProductId,
                    Quantity = od.Quantity,
                    UnitPrice = od.UnitPrice
                }).ToList(),
                PaymentQuantity = Total
            };

            try
            {
                
                var resultado = await _orderApiService.CreateOrderAsync(orderDto); // ahora devuelve string
                
                MessageBox.Show("Pedido registrado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Excepción al registrar pedido:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
