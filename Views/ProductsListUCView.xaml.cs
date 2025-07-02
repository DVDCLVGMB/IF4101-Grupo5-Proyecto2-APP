using Steady_Management.WPF.Views;
using Steady_Management_App.Models;
using Steady_Management_App.Models.Steady_Management_App.Models;
using Steady_Management_App.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Steady_Management_App.Views
{
    public partial class ProductListUCView : UserControl
    {
        private readonly ProductService _productService = new();
        private readonly CategoryService _categoryService = new();

        // ObservableCollection para actualizar la UI automáticamente
        public ObservableCollection<Product> Products { get; set; } = new();

        public ProductListUCView()
        {
            InitializeComponent();
            ProductsListView.ItemsSource = Products; // Binding una sola vez
            Loaded += ProductListUCView_Loaded;
            AplicarRestriccionesPorRol();
        }

        private void AplicarRestriccionesPorRol()
        {
            int roleId = UserSession.RoleId;

            if (roleId == 21) // Empleado
            {
                // Ocultar botón de agregar producto
                var addButton = FindButtonInHeader(this, "Agregar Producto");
                if (addButton != null)
                    addButton.Visibility = Visibility.Collapsed;

                // Ocultar la última columna (acciones)
                if (ProductsListView.View is GridView gridView && gridView.Columns.Count > 0)
                {
                    gridView.Columns.RemoveAt(gridView.Columns.Count - 1);
                }
            }
        }

        // Buscar botón dentro del header por su contenido
        private Button? FindButtonInHeader(DependencyObject parent, string contenido)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is Button btn && btn.Content?.ToString()?.Contains(contenido) == true)
                    return btn;

                var result = FindButtonInHeader(child, contenido);
                if (result != null) return result;
            }

            return null;
        }

        private async void ProductListUCView_Loaded(object sender, RoutedEventArgs e)
        {
            AplicarRestriccionesPorRol();
            await LoadProducts(); // Carga inicial de productos
        }





        // Carga productos desde el servicio, con categoría
        private async Task LoadProducts()
        {
            var products = await _productService.GetProductsAsync();
            var categories = await _categoryService.GetCategoriesAsync();

            Products.Clear(); // Limpiar la lista observable

            foreach (var p in products)
            {
                var cat = categories.FirstOrDefault(c => c.CategoryId == p.CategoryId);
                p.CategoryName = cat?.Description ?? "Desconocido";
                Products.Add(p); // Agregar productos uno por uno
            }
        }

        // Agregar nuevo producto
        private async void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            var form = new ProductDetailWindowView();
            bool? result = form.ShowDialog();

            if (result == true)
                await LoadProducts(); // Esperar recarga real
        }

        // Editar producto
        private async void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            var product = (Product)((FrameworkElement)sender).Tag;
            var form = new ProductDetailWindowView(product.ProductId);
            bool? result = form.ShowDialog();

            if (result == true)
                await LoadProducts(); // Esperar recarga real
        }

        // Eliminar producto
        private async void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            var product = (Product)((FrameworkElement)sender).Tag;

            var confirmDialog = new DeleteConfirmationDialog();
            confirmDialog.Owner = Window.GetWindow(this);
            confirmDialog.ShowDialog();

            if (confirmDialog.DeleteConfirmed)
            {
                await _productService.DeleteProductAsync(product.ProductId);
                await LoadProducts(); // Esperar recarga real
            }
        }

        // Redimensionamiento automático de columnas
        private void ProductsListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ProductsListView.View is GridView gridView && gridView.Columns.Count > 0)
            {
                double totalWidth = ProductsListView.ActualWidth - 35; // scrollbar
                double actionColumnWidth = 100;
                totalWidth -= actionColumnWidth;

                int columnsToResize = gridView.Columns.Count - 1;
                double columnWidth = totalWidth / columnsToResize;

                for (int i = 0; i < gridView.Columns.Count; i++)
                {
                    if (i == gridView.Columns.Count - 1)
                        gridView.Columns[i].Width = actionColumnWidth;
                    else
                        gridView.Columns[i].Width = columnWidth;
                }
            }
        }
    }
}
