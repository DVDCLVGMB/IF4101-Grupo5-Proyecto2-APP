using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Steady_Management_App.Models;
using Steady_Management_App.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

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
        private void AddProduct(OrderDetail detail)
        {
            OrderDetails.Add(detail);
            RecalculateTotals();
        }

        [RelayCommand]
        private void RemoveProduct(OrderDetail detail)
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
        private async Task CreateOrderAsync()
        {
            var order = new Order
            {
                ClientId = ClientId,
                CityId = CityId,
                EmployeeId = 4, // quemado según lo acordado
                OrderDate = OrderDate,
                OrderDetails = OrderDetails.ToList(),
                PaymentMethodId = PaymentMethodId,
                CreditCardNumber = PaymentMethodId == 2 ? CreditCardNumber : null,
                PaymentDate = OrderDate
            };

            await _orderApiService.CreateOrderAsync(order);
        }
    }
}

