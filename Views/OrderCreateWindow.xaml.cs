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
        //private string PaymentMethod = "Efectivo";
        private string? CardNumber = null;
        private readonly CreateOrderViewModel _viewModel;
        private readonly ProductService _productService;
        private readonly InventoryService _inventoryService;


        public OrderCreateWindow(OrderDTO order)
        {
            try
            {
                InitializeComponent();
                Debug.WriteLine("¡OrderCreateWindow fue cargado!");

                _order = order;
                _productService = new ProductService();
                _inventoryService = new InventoryService();


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
            var inventarios = await _inventoryService.GetInventoriesAsync();

            // Crear un HashSet con los IDs de productos que tienen inventario disponible (> 0)
            var productosConStock = new HashSet<int>(
                inventarios
                    .Where(inv => inv.ItemQuantity > 0)
                    .Select(inv => inv.ProductId)
            );

            // Filtrar detalles para incluir solo productos con inventario
            foreach (var i in _order.OrderDetails)
            {
                if (!productosConStock.Contains(i.ProductId))
                {
                    MessageBox.Show($"El producto '{i.ProductId}' no tiene inventario disponible y será excluido del pedido.",
                                    "Producto sin inventario", MessageBoxButton.OK, MessageBoxImage.Information);
                    continue;
                }

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
            _viewModel.RecalculateTotals();
            UpdateTotals();

            _viewModel.PaymentMethodId = 1;
            CardNumberTextBox.IsEnabled = false;
            efectivoRadio.IsChecked = true;
        }



        private async void OrderGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var editedDetail = e.Row.Item as OrderDetailDTO;
            if (editedDetail == null) return;

            int newQuantity = 0;
            if (e.EditingElement is TextBox tb && int.TryParse(tb.Text, out newQuantity))
            {
                // Usar InventoryService para obtener inventario
                var inventoryList = await _inventoryService.GetInventoriesAsync();

                // Buscar el inventario para el producto editado
                var inventory = inventoryList.FirstOrDefault(inv => inv.ProductId == editedDetail.ProductId);

                if (inventory == null || inventory.ItemQuantity < newQuantity)
                {
                    MessageBox.Show($"No hay suficiente inventario para el producto {editedDetail.ProductName}. Stock disponible: {inventory?.ItemQuantity ?? 0}.",
                                    "Inventario insuficiente", MessageBoxButton.OK, MessageBoxImage.Warning);

                    // Opcional: revertir el cambio a la cantidad disponible
                    editedDetail.Quantity = inventory?.ItemQuantity ?? 0;

                    // Refrescar el DataGrid para mostrar el cambio
                    OrderGrid.Items.Refresh();
                }
                else
                {
                    // Si está ok, actualizar totales normalmente
                    UpdateTotals();
                }
            }
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
                // Asignar el valor correcto al ViewModel según la selección
                _viewModel.PaymentMethodId = method == "Tarjeta" ? 2 : 1;

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
                
                _viewModel.CreditCardNumber = CardNumberTextBox.Text.Trim();
                await _viewModel.FinalizarPedidoAsync();

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
