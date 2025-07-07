using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Steady_Management_App.Models;
using Steady_Management_App.Services;
using Steady_Management_App.ViewModels;

namespace Steady_Management_App.Views
{
    /// <summary>
    /// Lógica de interacción para OrderCreateWindow.xaml
    /// </summary>
    public partial class OrderCreateWindow : UserControl
    {
        private OrderDTO _order;
        private string PaymentMethod = "Efectivo";
        private string? CardNumber = null;
        private readonly CreateOrderViewModel _viewModel;

        public OrderCreateWindow(OrderDTO order)
        {
            InitializeComponent();
            _order = order;
            _viewModel = new CreateOrderViewModel(new OrderApiService(new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7284/")
            }));

            _viewModel.ClientId = order.Client.ClientId;
            _viewModel.CityId = order.Client.CityId;
            _viewModel.OrderDetails = new ObservableCollection<OrderDetail>(order.Items.Select(i =>
                new OrderDetail
                {
                    ProductId = i.Product.ProductId,
                   // ProductCode = i.Product.ProductCode,
                    ProductName = i.Product.ProductName,
                    UnitPrice = i.Product.Price,
                    Quantity = i.Quantity,
                    IsTaxable = i.Product.IsTaxable,
                    //TaxAmount = i.Product.TaxAmount
                }));
            _viewModel.PaymentMethodId = 1;

            DataContext = _viewModel;

            OrderGrid.ItemsSource = _order.Items;
            OrderGrid.CellEditEnding += OrderGrid_CellEditEnding;
            UpdateTotals();
        }


        private void OrderGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            // Puedes validar cantidades aquí también si lo necesitas
            UpdateTotals();
            Dispatcher.InvokeAsync(UpdateTotals);
        }

        private void UpdateTotals()
        {
            TotalTaxText.Text = $"Impuesto: ₡{_order.TotalTaxes:N2}";
            TotalText.Text = $"Total: ₡{_order.Total:N2}";
        }

        private void PaymentMethod_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton rb && rb.Tag is string method)
            {
                PaymentMethod = method;

                if (CardNumberTextBox != null)
                {
                    CardNumberTextBox.IsEnabled = method == "Tarjeta";
                }
            }
        }

        private async void FinalizarPedido_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.CreditCardNumber = CardNumberTextBox.Text.Trim();
            await _viewModel.FinalizarPedidoAsync();
        }

    }


}
