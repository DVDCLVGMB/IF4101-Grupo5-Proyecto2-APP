using System;
using System.Collections.ObjectModel;
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

        [ObservableProperty] private ObservableCollection<OrderDetail> orderDetails = new();
        [ObservableProperty] private decimal subtotal;
        [ObservableProperty] private decimal taxes;
        [ObservableProperty] private decimal total;

        public CreateOrderViewModel(OrderApiService orderApiService)
        {
            _orderApiService = orderApiService;
        }

        [RelayCommand]
        public void AddProduct(OrderDetail detail)
        {
            OrderDetails.Add(detail);
            RecalculateTotals();
        }

        [RelayCommand]
        public void RemoveProduct(OrderDetail detail)
        {
            OrderDetails.Remove(detail);
            RecalculateTotals();
        }

        private void RecalculateTotals()
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
                MessageBox.Show("Debe ingresar un número de tarjeta válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (OrderDetails.Count == 0)
            {
                MessageBox.Show("Debe agregar al menos un producto al pedido.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                Items = OrderDetails.Select(od => new OrderItemDTO
                {
                    Product = new ProductDTO
                    {
                        ProductId = od.ProductId,
                        ProductName = od.ProductName,
                        Price = od.UnitPrice,
                        IsTaxable = od.IsTaxable,
                        // Asigna otras propiedades necesarias de ProductDTO aquí
                    },
                    Quantity = od.Quantity
                }).ToList()
            };

            try
            {
                var created = await _orderApiService.CreateOrderAsync(orderDto); // Cambiado de 'order' a 'orderDto'
                if (created)
                {
                    MessageBox.Show("Pedido registrado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("No se pudo registrar el pedido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Excepción", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
