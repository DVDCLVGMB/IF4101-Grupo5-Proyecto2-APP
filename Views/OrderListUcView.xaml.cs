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

        //private void EditButton_Click(object sender, RoutedEventArgs e)
        //{
        //    var dto = (OrderDisplayModel)((Button)sender).Tag;
        //    var win = new OrderFormWindow(dto.OrderId);
        //    win.Owner = Window.GetWindow(this);
        //    if (win.ShowDialog() == true)
        //        _ = LoadOrders();
        //}

        //private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        //{
        //    var dto = (OrderDisplayModel)((Button)sender).Tag;

        //    var confirm = MessageBox.Show(
        //        "¿Eliminar este pedido?",
        //        "Confirmar",
        //        MessageBoxButton.YesNo,
        //        MessageBoxImage.Question);

        //    if (confirm != MessageBoxResult.Yes) return;

        //    bool ok = await _orderSvc.DeleteOrderAsync(dto.OrderId);
        //    if (ok)
        //        _ = LoadOrders();
        //    else
        //        MessageBox.Show("Error al eliminar el pedido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //}
    }
}
