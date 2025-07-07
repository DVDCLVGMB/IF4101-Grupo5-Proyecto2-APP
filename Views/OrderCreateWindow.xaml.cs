using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using Steady_Management_App.DTOs;
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
        private readonly ProductService _productService;


        public OrderCreateWindow(OrderDTO order)
        {
            try
            {
                InitializeComponent();
                Debug.WriteLine("¡OrderCreateWindow fue cargado!");

                _order = order;
                _productService = new ProductService();

                _viewModel = new CreateOrderViewModel(new OrderApiService());


                _viewModel.ClientId = order.Client.ClientId;
                _viewModel.CityId = order.Client.CityId;
                _viewModel.PaymentMethodId = 1;

                DataContext = _viewModel;
                OrderGrid.CellEditEnding += OrderGrid_CellEditEnding;

                LoadDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear la vista de pedido:\n" + ex.Message, "Error crítico", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private async void LoadDataAsync()
        {
            var productosDisponibles = await _productService.GetProductsAsync();

            foreach (var i in _order.OrderDetails)
            {
                var producto = productosDisponibles.FirstOrDefault(p => p.ProductId == i.ProductId);
                _viewModel.OrderDetails.Add(new OrderDetailDTO
                {
                    ProductId = i.ProductId,
                    ProductName = producto?.ProductName ?? "(Desconocido)",
                    UnitPrice = i.UnitPrice,
                    Quantity = i.Quantity,
                    IsTaxable = i.IsTaxable
                });
            }

            OrderGrid.ItemsSource = _viewModel.OrderDetails;
            // Recalcula los totales después de llenar
            _viewModel.RecalculateTotals();
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
            TotalTaxText.Text = $"Impuesto: ₡{_viewModel.Taxes:N2}";
            TotalText.Text = $"Total: ₡{_viewModel.Total:N2}";
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
            try
            {
                MessageBox.Show("Cantidad de productos antes de enviar: " + _viewModel.OrderDetails.Count);

                _viewModel.CreditCardNumber = CardNumberTextBox.Text.Trim();
                await _viewModel.FinalizarPedidoAsync();

                MessageBox.Show("Pedido enviado correctamente");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al finalizar el pedido:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                // IMPORTANTE: log interno
                MessageBox.Show("Error detallado:");
                MessageBox.Show(ex.ToString());
            }
        }


    }


}
