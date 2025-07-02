using System.Windows;
using System.Windows.Controls;
using Steady_Management_App.DTOs;
using Steady_Management_App.Services;
using Steady_Management_App.ViewModels;

namespace Steady_Management_App.Views
{
    public partial class OrderProductSelectUcView : UserControl
    {
        private readonly ProductListViewModel _viewModel;

        public ProductDTO? SelectedProduct { get; private set; }

        public OrderProductSelectUcView()
        {
            InitializeComponent();
            _viewModel = new ProductListViewModel(new ProductService(), new CategoryService());
            DataContext = _viewModel;
        }


        private void Agregar_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedProduct != null)
            {
                SelectedProduct = _viewModel.SelectedProduct;

                ProductDataGrid.Visibility = Visibility.Collapsed;
                SelectedProductPanel.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Seleccione un producto primero.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            SelectedProduct = null;
            _viewModel.SelectedProduct = null;

            ProductDataGrid.Visibility = Visibility.Visible;
            SelectedProductPanel.Visibility = Visibility.Collapsed;
        }

        private void Select_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedProduct != null)
            {
                MessageBox.Show($"Producto '{SelectedProduct.ProductName}' agregado. Aquí puedes continuar con el pedido.");
                // Aquí podrías hacer la navegación al siguiente UserControl, por ejemplo para confirmar pedido.
            }
            else
            {
                MessageBox.Show("Debe seleccionar un producto primero.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
