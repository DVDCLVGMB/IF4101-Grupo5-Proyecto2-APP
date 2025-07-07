// Views/OrderListUcView.xaml.cs
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Steady_Management_App.Models;
using Steady_Management_App.Services;
using Steady_Management_App.Views; // para OrderFormWindow

namespace Steady_Management_App.Views
{
    public partial class OrderListUcView : UserControl
    {
        private readonly OrderApiService _orderSvc = new();
        private readonly ClientApiService _clientSvc = new();
        private readonly EmployeeService _employeeSvc = new();
        private readonly CityApiService _citySvc = new();

        public OrderListUcView()
        {
            InitializeComponent();
            _ = LoadOrders();
        }

        // Views/OrderListUcView.xaml.cs
        private async Task LoadOrders()
        {
            var orders = await _orderSvc.GetOrdersAsync();    // sólo trae IDs + OrderDate
            var clients = await _clientSvc.GetClientsAsync();   // trae { Id, Name }
            var employees = await _employeeSvc.GetEmployeesAsync();
            var cities = await _citySvc.GetCitiesAsync();

            var display = orders.Select(o => new OrderDisplayModel
            {
                OrderId = o.OrderId,
                ClientName = clients
                                  .FirstOrDefault(c => c.ClientId == o.ClientId)?.CompanyName
                              ?? "<desconocido>",
                EmployeeName = employees
                                  .FirstOrDefault(e => e.EmployeeId == o.EmployeeId)?.EmployeeName
                              ?? "<desconocido>",
                CityName = cities
                                  .FirstOrDefault(ci => ci.CityId == o.CityId)?.CityName
                              ?? "<desconocido>",
                OrderDate = o.OrderDate
            }).ToList();

            OrdersDataGrid.ItemsSource = display;
        }


        private void RefreshButton_Click(object sender, RoutedEventArgs e)
            => _ = LoadOrders();

        // BORRAR
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).Tag;
            if (MessageBox.Show(
                    "¿Eliminar este pedido?",
                    "Confirmar",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question)
                != MessageBoxResult.Yes) return;

            await _orderSvc.DeleteOrderAsync(id);
            await LoadOrders();
        }

        // EDITAR (Update)
        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).Tag;
            var detail = await _orderSvc.GetOrderAsync(id);
            if (detail is null) return;

            // Mapea al OrderDTO o al OrderRequestDto que uses en tu formulario:
            var orderToEdit = new OrderDTO
            {
                OrderId = detail.OrderId,
                ClientId = detail.ClientId,
                EmployeeId = detail.EmployeeId,
                CityId = detail.CityId,
                OrderDate = detail.OrderDate
            };

            var win = new OrderFormWindow(orderToEdit);
            win.Owner = Window.GetWindow(this);
            if (win.ShowDialog() == true)
                await LoadOrders();
        }
    }
}
