using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Steady_Management_App.Models;
using Steady_Management_App.Services;

namespace Steady_Management_App.Views
{
    public partial class ProductListUCView : UserControl
    {
        private readonly ProductService _productService = new();
        private readonly CategoryService _categoryService = new();

        public ProductListUCView()
        {
            InitializeComponent();
            LoadProducts();
        }

        private async void LoadProducts()
        {
            var products = await _productService.GetProductsAsync();
            var categories = await _categoryService.GetCategoriesAsync();

            foreach (var p in products)
            {
                var cat = categories.FirstOrDefault(c => c.CategoryId == p.CategoryId);
                p.CategoryName = cat?.Description ?? "Desconocido";
            }

            ProductsDataGrid.ItemsSource = products;
        }


        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            var form = new ProductDetailWindowView();
            form.ShowDialog();
            LoadProducts();
        }

        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            // Toma el producto seleccionado en el grid
            if (ProductsDataGrid.SelectedItem is Product product)
            {
                var form = new ProductDetailWindowView(product.ProductId);
                form.ShowDialog();
                LoadProducts();
            }
            else
            {
                MessageBox.Show("Por favor selecciona un producto primero.",
                                "Editar producto",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
            }
        }


        private async void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsDataGrid.SelectedItem is Product product)
            {
                var confirm = MessageBox.Show(
                    "¿Desea eliminar este producto?",
                    "Confirmar eliminación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (confirm == MessageBoxResult.Yes)
                {
                    await _productService.DeleteProductAsync(product.ProductId);
                    LoadProducts();
                }
            }
            else
            {
                MessageBox.Show("Por favor selecciona un producto primero.",
                                "Eliminar producto",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
            }
        }
    }
}
