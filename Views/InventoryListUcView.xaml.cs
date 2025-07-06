using System.Windows;
using System.Windows.Controls;
using Steady_Management_App.DTOs;
using Steady_Management_App.Services;

namespace Steady_Management_App.Views
{
    public partial class InventoryListUcView : UserControl
    {
        private readonly InventoryService _svc = new();

        public InventoryListUcView()
        {
            InitializeComponent();
            LoadInventories();
        }

        private readonly ProductService _prodSvc = new();
        private readonly InventoryService _invSvc = new();

        private async void LoadInventories()
        {
            var list = await _invSvc.GetInventoriesAsync();   // ya con ItemQuantity, etc.
            var prods = await _prodSvc.GetProductsAsync();

            foreach (var inv in list)
            {
                var p = prods.FirstOrDefault(x => x.ProductId == inv.ProductId);
                inv.ProductName = p?.ProductName ?? "<desconocido>";
            }

            InventoryDataGrid.ItemsSource = list;
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
            => LoadInventories();

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var dto = btn?.Tag as InventoryDTO;
            if (dto == null) return;

            var win = new InventoryFormWindow(dto.InventoryId);
            win.Owner = Window.GetWindow(this);
            if (win.ShowDialog() == true)
                LoadInventories();
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var dto = btn?.Tag as InventoryDTO;
            if (dto == null) return;

            var confirm = MessageBox.Show(
                "¿Eliminar este inventario?",
                "Confirmar",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (confirm != MessageBoxResult.Yes) return;

            bool ok = await _svc.DeleteInventoryAsync(dto.InventoryId);
            if (ok)
                LoadInventories();
            else
                MessageBox.Show("Error al eliminar.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void AddInventoryButton_Click(object sender, RoutedEventArgs e)
        {
            var win = new InventoryFormWindow();  // constructor sin parámetros para "nuevo"
            win.Owner = Window.GetWindow(this);
            if (win.ShowDialog() == true)
                LoadInventories();
        }
    }
}
