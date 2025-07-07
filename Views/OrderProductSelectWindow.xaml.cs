using System.Windows;
using System.Windows.Controls;
using PedidoApp;
using Steady_Management_App.DTOs;
using Steady_Management_App.Services;
using Steady_Management_App.ViewModels;
using Steady_Management_App.ViewModels.Utility;

namespace Steady_Management_App.Views
{
    public partial class OrderProductSelectUcView : UserControl
    {
        private readonly ProductListViewModel _viewModel;

        private List<ProductDTO> SelectedProducts { get; set; } = new();
        private bool hasConfirmedSelection = false;

        public OrderProductSelectUcView()
        {
            InitializeComponent();
            _viewModel = new ProductListViewModel(new ProductService(), new CategoryService());
            DataContext = _viewModel;
        }

        private void Agregar_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedProducts.Any())
            {
                hasConfirmedSelection = true;

                ProductDataGrid.Visibility = Visibility.Collapsed;
                SelectedProductPanel.Visibility = Visibility.Visible;

                // Refresca la lista de seleccionados
                SelectedProductsList.ItemsSource = null;
                SelectedProductsList.ItemsSource = SelectedProducts;
            }
            else
            {
                MessageBox.Show("Seleccione al menos un producto antes de continuar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            hasConfirmedSelection = false;

            // Desmarcar visualmente los CheckBox
            foreach (var product in _viewModel.Products)
            {
                product.IsSelected = false;
            }

            SelectedProducts.Clear();
            ProductDataGrid.Items.Refresh(); // Actualiza el DataGrid visual

            ProductDataGrid.Visibility = Visibility.Visible;
            SelectedProductPanel.Visibility = Visibility.Collapsed;
        }



        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            if (!hasConfirmedSelection || !SelectedProducts.Any())
            {
                MessageBox.Show("Debe seleccionar al menos un producto y presionar el botón 'Seleccionar Producto' antes de continuar.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (OrderSession.SelectedClient == null)
            {
                MessageBox.Show("No hay cliente seleccionado. Vuelva al paso anterior.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var order = new OrderDTO
            {
                Client = OrderSession.SelectedClient,
                Items = SelectedProducts.Select(p => new OrderItemDTO { Product = p, Quantity = 1 }).ToList()
            };

            var productosUc = new OrderCreateWindow(order);
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.ContenidoArea.Content = productosUc;
            }
        }


        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox cb && cb.DataContext is ProductDTO product && !SelectedProducts.Contains(product))
            {
                SelectedProducts.Add(product);
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox cb && cb.DataContext is ProductDTO product && SelectedProducts.Contains(product))
            {
                SelectedProducts.Remove(product);
            }
        }

    }
}
