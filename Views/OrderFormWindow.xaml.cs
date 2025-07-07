using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using Steady_Management_App.DTOs;   // para OrderDTO
using Steady_Management_App.Models; // Client, Employee, City
using Steady_Management_App.Services;

namespace Steady_Management_App.Views
{
    public partial class OrderFormWindow : Window, INotifyPropertyChanged
    {
        private readonly OrderApiService _orderSvc = new();
        private readonly ClientApiService _clientSvc = new();
        private readonly EmployeeService _employeeSvc = new();
        private readonly CityApiService _citySvc = new();

        public OrderDTO Order { get; set; }

        public ObservableCollection<Client> Clients { get; set; }
        public ObservableCollection<Employee> Employees { get; set; }
        public ObservableCollection<City> Cities { get; set; }

        // Constructor "Nuevo"
        public OrderFormWindow()
        {
            InitializeComponent();
            Order = new OrderDTO { OrderDate = DateTime.Today };
            DataContext = this;
            _ = LoadLookups();
        }

        // Constructor "Editar"
        public OrderFormWindow(OrderDTO existing) : this()
        {
            Order = existing;
            OnPropertyChanged(nameof(Order));
        }

        private async Task LoadLookups()
        {
            var cl = await _clientSvc.GetClientsAsync();
            var em = await _employeeSvc.GetEmployeesAsync();
            var ci = await _citySvc.GetCitiesAsync();

            Clients = new ObservableCollection<Client>(cl);
            Employees = new ObservableCollection<Employee>(em);
            Cities = new ObservableCollection<City>(ci);

            OnPropertyChanged(nameof(Clients));
            OnPropertyChanged(nameof(Employees));
            OnPropertyChanged(nameof(Cities));
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Mapear campos básicos
                var rq = new OrderRequestDto
                {
                    OrderId = Order.OrderId,
                    ClientId = Order.ClientId,
                    EmployeeId = Order.EmployeeId,
                    CityId = Order.CityId,
                    OrderDate = Order.OrderDate,

                    OrderDetails = Order.OrderDetails
                                    .Select(d => new OrderDetailRequestDto
                                    {
                                        ProductId = d.ProductId,
                                        Quantity = d.Quantity
                                    })
                                    .ToList()

                };

                if (rq.OrderId == 0)
                    await _orderSvc.CreateOrderAsync(Order);
                else
                    await _orderSvc.UpdateOrderAsync(rq.OrderId, rq);

                DialogResult = true;
            }
            catch (HttpRequestException ex)
            {
                // ya leenás y lanzás el contenido de error, así ves el mensaje exacto
                MessageBox.Show($"Error al guardar:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string prop) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
