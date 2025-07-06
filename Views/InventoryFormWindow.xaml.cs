// InventoryFormWindow.xaml.cs
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Steady_Management_App.Models;    // <-- aquí tu modelo Category
using Steady_Management_App.DTOs;
using Steady_Management_App.Services;
using System.Windows.Input;

namespace Steady_Management_App.Views
{
    public partial class InventoryFormWindow : Window
    {
        // servicios
        private readonly InventoryService _invSvc = new();
        private readonly ProductService _prodSvc = new();
        private readonly CategoryService _catSvc = new();

        // regex para digitos
        private readonly Regex _digits = new(@"^\d+$");

        // campos de estado
        private List<Category> _categories = new();
        private InventoryResponseDTO? _editing;

        private async Task InitializeFormAsync(int inventoryId)
        {
            // 1) Cargo todas las categorías
            _categories = await _catSvc.GetCategoriesAsync();

            // 2) Cargo TODOS los productos
            var allProducts = await _prodSvc.GetProductsAsync();

            // 3) Cargo TODOS los inventarios para saber cuáles ya existen
            var existingInvs = await _invSvc.GetInventoriesAsync();

            // 4) Dejo sólo los productos que NO tienen inventario asignado
            var available = allProducts
                .Where(p => !existingInvs.Any(inv => inv.ProductId == p.ProductId))
                .ToList();

            // 5) Si esta EDITANDO un inventario existente, 
            //    debe añadir el producto que ya tiene inventario
            if (inventoryId > 0)
            {
                var current = existingInvs.FirstOrDefault(i => i.InventoryId == inventoryId);
                if (current != null)
                {
                    var prod = allProducts.First(p => p.ProductId == current.ProductId);
                    available.Add(prod);
                }
            }

            // 6) Asigno al ComboBox
            ProductComboBox.ItemsSource = available;
            ProductComboBox.DisplayMemberPath = nameof(ProductDTO.ProductName);
            ProductComboBox.SelectedValuePath = nameof(ProductDTO.ProductId);

            // 7) Suscribo el SelectionChanged (ahora que ya hay items)
            ProductComboBox.SelectionChanged += ProductComboBox_SelectionChanged;

            // 8) Si edición, cargo valores existentes
            if (inventoryId > 0)
                await LoadExistingAsync(inventoryId);
        }

        // Constructor
        public InventoryFormWindow(int inventoryId = 0)
        {
            InitializeComponent();
            SizeComboBox.IsEnabled = false;

            Loaded += async (_, __) => await InitializeFormAsync(inventoryId);
        }


        private async Task LoadCategoriesAsync()
        {
            // carga tu modelo Category
            _categories = await _catSvc.GetCategoriesAsync();
        }

        private async Task LoadProductsAsync()
        {
            var prods = await _prodSvc.GetProductsAsync();
            ProductComboBox.ItemsSource = prods;
            ProductComboBox.DisplayMemberPath = nameof(ProductDTO.ProductName);
            ProductComboBox.SelectedValuePath = nameof(ProductDTO.ProductId);
        }

        private async Task LoadExistingAsync(int id)
        {
            var dto = await _invSvc.GetByIdAsync(id);
            if (dto == null) return;
            _editing = dto;

            ProductComboBox.SelectedValue = dto.ProductId;
            QuantityBox.Text = dto.ItemQuantity.ToString();
            LimitBox.Text = dto.LimitUntilRestock.ToString();
            if (!string.IsNullOrEmpty(dto.Size))
                SizeComboBox.SelectedItem = SizeComboBox
                                                .Items
                                                .OfType<ComboBoxItem>()
                                                .FirstOrDefault(x => (string)x.Content == dto.Size);
        }

        private void ProductComboBox_SelectionChanged(object _, SelectionChangedEventArgs __)
        {
            // DEBUGGING: confirma que _categories esté rellenado
            System.Diagnostics.Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Categories count = {_categories.Count}");

            if (ProductComboBox.SelectedItem is not Product prod)
            {
                SizeComboBox.IsEnabled = false;
                return;
            }

            // busca la categoría en tu lista de Category
            var catDesc = _categories
                            .FirstOrDefault(c => c.CategoryId == prod.CategoryId)
                            ?.Description;

            bool needsSize = catDesc == "Ropa"
                          || catDesc == "Calzado";

            SizeComboBox.IsEnabled = needsSize;
            if (!needsSize)
                SizeComboBox.SelectedIndex = -1;
        }

        private void IntegerOnly(object _, TextCompositionEventArgs e)
            => e.Handled = !_digits.IsMatch(e.Text);

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (ProductComboBox.SelectedItem is not Product prod)
            {
                MessageBox.Show("Seleccione un producto.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!int.TryParse(QuantityBox.Text, out var qty)
             || !int.TryParse(LimitBox.Text, out var lim))
            {
                MessageBox.Show("Cantidad o límite inválido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var dto = new InventoryResponseDTO
            {
                InventoryId = _editing?.InventoryId ?? 0,
                ProductId = prod.ProductId,
                ItemQuantity = qty,
                LimitUntilRestock = lim,
                Size = SizeComboBox.IsEnabled
                                  ? (SizeComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString()
                                  : null
            };

            bool ok = dto.InventoryId == 0
                   ? await _invSvc.CreateInventoryAsync(dto)
                   : await _invSvc.UpdateInventoryAsync(dto);

            if (ok) DialogResult = true;
            else MessageBox.Show("Error al guardar.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}
