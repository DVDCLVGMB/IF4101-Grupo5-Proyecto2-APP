// ViewModels/OrdersListViewModel.cs
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Steady_Management_App.Models;
using Steady_Management_App.Services;

namespace Steady_Management_App.ViewModels
{
    public class OrdersListViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<OrderDisplayModel> Orders { get; private set; }

        private readonly OrderApiService _orderSvc = new();
        private readonly ClientApiService _clientSvc = new();
        private readonly EmployeeService _employeeSvc = new();
        private readonly CityApiService _citySvc = new();

        public OrdersListViewModel()
        {
            _ = LoadOrdersAsync();
        }

        private async Task LoadOrdersAsync()
        {
            Debug.WriteLine("→ Inicio LoadOrdersAsync");
            var list = await _orderSvc.GetOrdersAsync();
            Debug.WriteLine($"→ List count: {list.Count}");

            var orderTask = _orderSvc.GetOrdersAsync();
            var clientTask = _clientSvc.GetClientsAsync();
            var employeeTask = _employeeSvc.GetEmployeesAsync();
            var cityTask = _citySvc.GetCitiesAsync();

            await Task.WhenAll(orderTask, clientTask, employeeTask, cityTask);

            var clients = clientTask.Result.ToDictionary(c => c.ClientId, c => c.CompanyName);
            var employees = employeeTask.Result.ToDictionary(e => e.EmployeeId, e => e.EmployeeName);
            var cities = cityTask.Result.ToDictionary(c => c.CityId, c => c.CityName);

            var display = orderTask.Result.Select(o =>
            {
                // Cliente
                if (!clients.TryGetValue(o.ClientId, out var clientName))
                    clientName = "—";

                // Empleado
                if (!employees.TryGetValue(o.EmployeeId, out var employeeName))
                    employeeName = "—";

                // Ciudad
                if (!cities.TryGetValue(o.CityId, out var cityName))
                    cityName = "—";

                return new OrderDisplayModel
                {
                    OrderId = o.OrderId,
                    ClientName = clientName,
                    EmployeeName = employeeName,
                    CityName = cityName,
                    OrderDate = o.OrderDate
                };
            });

            Orders = new ObservableCollection<OrderDisplayModel>(display);
            OnPropertyChanged(nameof(Orders));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
